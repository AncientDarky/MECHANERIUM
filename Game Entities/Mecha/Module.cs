using System;
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
        ModuleStates ModuleState;

        [Header("Module General Properties")]
        [SerializeField] float Hitpoint;
        public float HITPOINT => Hitpoint;

        [SerializeField] int MaxHitpoint;
        public int MAXHP => MaxHitpoint;

        [SerializeField]public int ModuleLevel;
        internal bool Invulnerable;
        [SerializeField] int MaxModuleLevel;
        [SerializeField] Cost[] UpgradeCost;
        [SerializeField] Cost[] MaterialConsumption;
        [SerializeField] Cost[] OverloadedMaterialConsumption;


        public Action OnUpraded;
        public Action<ModuleStates> StateChanged;
        private void Awake()
        {
            StateChanged += ChangingState;
            Invulnerable = true;
        }
        public void IncreaseLevel()
        {
            OnUpraded?.Invoke();
        }
        public void ReduceHP(float Damage)
        {
            if (Invulnerable) { return; }

        }
        public void RegainHP(float HPGain)
        {

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

