using UnityEngine;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;
namespace Mechaerium
{
    public class ReactorModule : Module
    {
        [Header("ReactorProperties")]
        [SerializeField] internal float[] NormalSpeed;
        [SerializeField] internal float[] RunningSpeed;
        [SerializeField] internal float[] SelfRepairSpeed;
        [SerializeField] internal float[] AimingRotationSpeed;
        public void ReactorTick()
        {

        }
        #region UI Chartacter Sheet 
        public float[] TransferModuleValues(int Index)
        {
            if (Index > MaxModuleLevel)
            {
                Index = MaxModuleLevel;
            }
            float[] Values = new float[3];

            Values[0] = NormalSpeed[Index];
            Values[1] = SelfRepairSpeed[Index];
            Values[2] = AimingRotationSpeed[Index];

            return Values;
        }
        #endregion
    }
}
