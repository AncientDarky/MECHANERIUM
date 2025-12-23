using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Robitnekics;

namespace Mechaerium
{
    public class WeaponModule : Module
    {
        [SerializeField] internal FunctionalityType FuncType;
        [SerializeField] SphereCollider AutoEngageDetector;

        [SerializeField] public Damage BaseDamage;
        [SerializeField] public bool ToggleOn;
        [SerializeField]Cost InstallCost;
        [SerializeField] internal float[] AutoEngageRange;
        [SerializeField] float[] MaxProjectileTravelDistance;
        [SerializeField] public Vector3 AccuracyOffset;
        [SerializeField] float[] ReloadingDuration;

        public Animator WeaponAnimator;
        [SerializeField] internal VisualEffect Visuals;
        public Vector3 MouseWorldLocationCurrent;
        internal List<Robites> Targets;
        public Robites CurrentTarget;

        private void Awake()
        {
            Invulnerable = false;
            ToggleOn = true;
            Targets = new List<Robites>();

            WeaponAnimator = GetComponent<Animator>();
            if(FuncType == FunctionalityType.Automatic)
            {
                AutoEngageDetector = this.gameObject.AddComponent<SphereCollider>();
                AutoEngageDetector.isTrigger = true;
                AutoEngageDetector.radius = AutoEngageRange[ModuleLevel];

            }
        }
        private void OnTriggerEnter(Collider other)
        {
            switch(other.gameObject.tag)
            {
                case "Enemy":
                    if(other.gameObject.GetComponent<Robites>())
                    {


                        Debug.Log("Added");
                        Targets.Add(other.gameObject.GetComponent<Robites>());


                    }
                    if(CurrentTarget == null)
                    {


                        Debug.Log("Start Engaging from inactivity");
                        CurrentTarget = GetNewTarget();


                    }

                    WeaponStartFiring();
                    break;
                case "Node":
                    break;
                case "Loot":
                    break;
                case "LockedCrate":
                    break;
                default:
                    break;
            }
        }
        Robites GetNewTarget()
        {
            if(Targets.Count <= 0) { Debug.Log("No remaining targets Left"); return null; }
            Robites NewTarget = Targets[0];
            NewTarget.OnDeath += OnTargetDeath;
            return NewTarget;
        }
        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    if (other.gameObject.GetComponent<Robites>())
                    {
                        Debug.Log("Removed");
                        Targets.Remove(other.gameObject.GetComponent<Robites>());
                        if (CurrentTarget == other.gameObject.GetComponent<Robites>())
                        {  
                                Debug.Log("Gain new Target As Curent Target is exited range");
                                CurrentTarget = GetNewTarget();
                        }
                    }
                   
                    if (Targets.Count <= 0)
                    {
                        WeaponStopFiring();
                    }
                    break;
                case "Node":
                    break;
                case "Loot":
                    break;
                case "LockedCrate":
                    break;
                default:
                    break;
            }

        }
        public void OnTargetDeath(GameObject Target)
        {
            switch (Target.gameObject.tag)
            {
                case "Enemy":
                    Robites A;
                    if (A = Target.gameObject.GetComponent<Robites>())
                    {
                        Debug.Log("RemovedDied");
                        Targets.Remove(Target.gameObject.GetComponent<Robites>());
                        A.OnDeath -= OnTargetDeath;
                        CurrentTarget = null;
                    }
                    if (Targets.Count <= 0)
                    {

                        Debug.Log("No remaining Targets Left");
                        WeaponStopFiring();
                    }
                    else if(Targets.Count > 0)
                    {

                        Debug.Log("Remaining Targets left re engaging");
                        CurrentTarget = GetNewTarget();
                        WeaponStartFiring();
                    }
                    break;
                case "Node":
                    break;
                case "Loot":
                    break;
                case "LockedCrate":
                    break;
                default:
                    break;
            }

        }
        public void WeaponStartFiring()
        {
            if(ToggleOn)
            {

                Debug.Log("Toggeled On Weapon");
                if(InventoryHasBaseCostMaterial)
                {
                    Mecha.STORERAGE.IncreasingAmmunitionConsumption(MaterialConsumption[ModuleLevel].Material, MaterialConsumption[ModuleLevel].Value,true);
                    Debug.Log("Resource Avaialble");

                    WeaponAnimator.SetBool("IsFiring",true);

                    WeaponAnimator.SetBool("IsIdle", false);

                    Visuals.SetBool("Toggle",true);

                }
            }
        }
       
        public void WeaponStopFiring()
        {
          WeaponAnimator.SetBool("IsFiring", false);
          WeaponAnimator.SetBool("IsIdle", true);

            Visuals.SetBool("Toggle", false);

            Mecha.STORERAGE.IncreasingAmmunitionConsumption(MaterialConsumption[ModuleLevel].Material, MaterialConsumption[ModuleLevel].Value, false);
        }
        public void ToggleOnOff(bool Value)
        {
            ToggleOn = Value;
        }
        public void RemoveWeapon()
        {
            Destroy(this.gameObject);
        }
        private void LateUpdate()
        {
            if (FuncType == FunctionalityType.Automatic && CurrentTarget)
            {
                Vector3 Dir = CurrentTarget.transform.position - this.transform.position;
                float AngleY = Mathf.Atan2(Dir.x,Dir.z) * Mathf.Rad2Deg;
                this.gameObject.transform.rotation = Quaternion.Euler(0,AngleY,0); 
            }
        }
    }
}
