using UnityEngine;
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
    }
}
