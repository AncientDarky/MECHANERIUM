using UnityEngine;
namespace Mechaerium
{
    public class CPUModule : Module
    {
        [Header("Orbital Hacking")]
        [SerializeField] float[] HackingSpeed;
        [SerializeField] float[] HackingRange;

        private void Start()
        {
            Invulnerable = false;
        }
        public void CPUTick()
        {

        }
    }
}
