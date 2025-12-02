using UnityEngine;
namespace Mechaerium
{
    public class CoreModule : Module
    {
        [Header("Core Module Properties")]
        [SerializeField] float VisionRange;
        [SerializeField] int MaxLifeCapacity;
        [SerializeField] float RemainingLifeSupport;
        [SerializeField] int[] MaximumWeaponSlot;
        [SerializeField] Transform[] WeaponSlots;
        int RemainingSlots;
        [SerializeField] Cost[] SelfRepairConsumption;
        private void Start()
        {
            Invulnerable = false;

        }
        public void CoreModuleTick()
        {

        }
    }
}
