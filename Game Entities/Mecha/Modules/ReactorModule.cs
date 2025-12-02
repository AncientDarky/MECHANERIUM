using UnityEngine;
namespace Mechaerium
{
    public class ReactorModule : Module
    {
        [Header("ReactorProperties")]
        [SerializeField] float[] NormalSpeed;
        [SerializeField] float[] RunningSpeed;
        [SerializeField] float[] SelfRepairSpeed;
        [SerializeField] float[] GatheringSpeed;
        [SerializeField] float[] AimingRotationSpeed;
        public void ReactorTick()
        {

        }
    }
}
