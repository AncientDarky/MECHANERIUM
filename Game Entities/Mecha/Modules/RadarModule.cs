using UnityEngine;
namespace Mechaerium
{
    public class RadarModule : Module
    {
        [Header("Detection Properties")]
        [SerializeField] float[] RadarStrength;
        [SerializeField] float[] RadarRange;
        [SerializeField] float[] RadarCooldown;
        private void Start()
        {
            Invulnerable = false;
        }
        public void RadarTick()
        {

        }
    }
}
