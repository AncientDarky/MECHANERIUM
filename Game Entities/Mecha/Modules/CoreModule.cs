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

        public float REMAINLIFESUPP => RemainingLifeSupport;
        public int MAXWEAPONCOUNT => MaximumWeaponSlot[ModuleLevel];

        public float REMAINTIME => RemainingLifeSupport / MaterialConsumption[0].Value;

        [SerializeField] Transform[] WeaponSlots;
        int RemainingSlots;
        // self repair must be re programmed currectly
         
        private void Start()
        {
            Invulnerable = false;
            RemainingLifeSupport = MAXLIFECAP;

        }
        public void CoreModuleTick()
        {
            RemainingLifeSupport = Mathf.Clamp(RemainingLifeSupport - MaterialConsumption[0].Value * Time.deltaTime,0,MAXLIFECAP);
        }
        public void ReceivingLifeSupport(int AmountReceived)
        {
            RemainingLifeSupport = Mathf.Clamp(RemainingLifeSupport + AmountReceived ,0,MAXLIFECAP);
        }
        private void LateUpdate()
        {
            CoreModuleTick();
        }
        #region UI Character Sheet 
        public float[] TransferModuleValues(int Index)
        {
            if (Index > MaxModuleLevel)
            {
                Index = MaxModuleLevel;
            }

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
