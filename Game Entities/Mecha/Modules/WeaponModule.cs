using UnityEngine;
namespace Mechaerium
{
    public class WeaponModule : Module
    {
        [SerializeField] public Damage BaseDamage;
        [SerializeField] public bool ToggleOn;
        [SerializeField] public bool ManuallyControlled;
        [SerializeField]Cost InstallCost;
        [SerializeField] TargetDetection targetDetector;
        [SerializeField] float[] AutoEngageRange;
        [SerializeField] float[] MaxProjectileTravelDistance;
        [SerializeField] public Vector3 AccuracyOffset;
        [SerializeField] float[] ReloadingDuration;

        public Animator WeaponAnimator;

        public Vector3 MouseWorldLocationCurrent;

        private void Awake()
        {
            Invulnerable = false;
            ToggleOn = true;
            if(ManuallyControlled == false)
            {
                targetDetector.weaponModule = this;
            }
            WeaponAnimator = GetComponent<Animator>();
        }
        public void WeaponStartFiring()
        {
            if(ToggleOn)
            {
                Debug.Log("Toggeled On Weapon");
                if(InventoryHasBaseCostMaterial)
                {
                    Debug.Log("Resource Avaialble");

                    WeaponAnimator.SetBool("IsFiring",true);

                    WeaponAnimator.SetBool("IsIdle", false);

                }
            }
        }
        public void WeaponStopFiring()
        {
          WeaponAnimator.SetBool("IsFiring", false);
          WeaponAnimator.SetBool("IsIdle", true);
        }
        public void ToggleOnOff(bool Value)
        {
            ToggleOn = Value;
        }
        public void RemoveWeapon()
        {
            Destroy(this.gameObject);
        }

    }
}
