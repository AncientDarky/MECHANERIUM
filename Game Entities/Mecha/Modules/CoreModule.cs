using UnityEngine;
namespace Mechaerium
{
    public class CoreModule : Module
    {
        [Header("Core Module Properties")]
        [SerializeField] float VisionRange;
        [SerializeField] int[] MaxLifeCapacity;
        public int MAXLIFECAP => MaxLifeCapacity[ModuleLevel];

        [SerializeField] float RemainingLifeSupport;
        [SerializeField] int[] MaximumWeaponSlot;
        public int MAXWEAPONCOUNT => MaximumWeaponSlot[ModuleLevel];

        [SerializeField] Transform[] WeaponSlots;
        int RemainingSlots;
        // self repair must be re programmed currectly
         
        private void Start()
        {
            Invulnerable = false;

        }
        public void CoreModuleTick()
        {

        }

        #region UI Character Sheet 
        public float[] TransferModuleValues(int Index)
        {
            float[] Values = new float[4];
            Values[0] = MaxHitpoint[Index];
            Values[1] = MaxLifeCapacity[Index];
            Values[2] = MaximumWeaponSlot[Index];
            Values[3] = 404;

            return Values;
        }
        #endregion
    }
}
