using UnityEngine;
namespace Mechaerium
{
    public class ArmourModule : Module
    {
        [Header("Armour Properties")]
        [SerializeField] float[] HeatResistance;
        public float HEATRESIST => HeatResistance[ModuleLevel];
        [SerializeField] float[] PhysicalResistance;
        public float PHSICALRESIST => PhysicalResistance[ModuleLevel];

        [SerializeField] float[] ExplosionResistance;
        public float EXPLORESIST => ExplosionResistance[ModuleLevel];
        private void Start()
        {
            Invulnerable = false;
        }
    }
}
