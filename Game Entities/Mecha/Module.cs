using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
namespace Mechaerium
{
    [Serializable]
    public struct Cost
    {
        public MaterialTypes Material;
        public float Value;
    }
    public class Module : MonoBehaviour
    {
        internal ModuleStates ModuleState;

        [Header("Module General Properties")]
        [SerializeField] internal float Hitpoint;
        public float HITPOINT => Hitpoint;

        [SerializeField] internal int[] MaxHitpoint;
        public int MAXHP => MaxHitpoint[ModuleLevel];

        [SerializeField]public int ModuleLevel;
        internal bool Invulnerable;
        [SerializeField] int MaxModuleLevel;
        [SerializeField] Cost[] UpgradeCost;
        public float UPGRADECOSTAMT => UpgradeCost[0].Value;
        [SerializeField] internal Cost[] MaterialConsumption;
        [SerializeField] Cost[] OverloadedMaterialConsumption;
        public bool InventoryHasBaseCostMaterial => Mecha.STORERAGE.HasEnoughMaterial(MaterialConsumption[ModuleLevel].Material, MaterialConsumption[ModuleLevel].Value);
        public bool InventoryHasOverloadedCostMaterial => Mecha.STORERAGE.HasEnoughMaterial(OverloadedMaterialConsumption[ModuleLevel].Material, OverloadedMaterialConsumption[ModuleLevel].Value);


        public Action OnUpraded,OnRepaired;
        public Action<ModuleStates> StateChanged;
        private void Awake()
        {
            StateChanged += ChangingState;
            Invulnerable = true;
        }
       
        public void IncreaseLevel()
        {

            if (Mecha.STORERAGE.HasEnoughMaterial(UpgradeCost[0].Material, UpgradeCost[0].Value))
            {
                ModuleLevel = Mathf.Clamp(ModuleLevel + 1,0,MaxModuleLevel);
                OnUpraded?.Invoke();
            }
        }
        public void ReduceHP(float Damage)
        {
           
            if (Invulnerable) { return; }
            Hitpoint -= Damage;
            if (Hitpoint <= 0)
            {
                ChangingState(ModuleStates.Destroyed);
               
            }

        }
        public void RegainHP(float HPGain)
        {
            
            Hitpoint = Mathf.Clamp(Hitpoint + HPGain, 0, MaxHitpoint[ModuleLevel]);
            
        }
        public void ChangingState(ModuleStates ToState)
        {
            ModuleState = ToState;
            HandleState();
        }
        void HandleState()
        {
            switch(ModuleState)
            {
                case ModuleStates.None:

                    break;
                case ModuleStates.Idle:

                    break;
                case ModuleStates.Engaged:

                    break;
                case ModuleStates.Destroyed:

                    break;
            }
        }

    }
}

