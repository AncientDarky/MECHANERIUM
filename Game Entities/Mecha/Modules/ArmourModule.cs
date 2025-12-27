using UnityEngine;
namespace Mechaerium
{
    public class ArmourModule : Module
    {
        [Header("Armour Properties")]
        [SerializeField][Range(0.0f, 1.0f)] float[] HeatResistance;
        public float HEATRESIST => HeatResistance[ModuleLevel];
        [SerializeField][Range(0.0f, 1.0f)] float[] PhysicalResistance;
        public float PHSICALRESIST => PhysicalResistance[ModuleLevel];
        [SerializeField][Range(0.0f, 1.0f)] float[] Piercing;
        public float PIERCINGRESIST => Piercing[ModuleLevel];

        [SerializeField][Range(0.0f,1.0f)] float[] ExplosionResistance;
        public float EXPLORESIST => ExplosionResistance[ModuleLevel];

        ArmourDisplayer CharacterSheet_ArmourDisplayer;
        private void Start()
        {
            Invulnerable = false;
            CharacterSheet_ArmourDisplayer = FindAnyObjectByType<ArmourDisplayer>();
        }
        public float[] TransferModuleValues (int AtLevel)
        {
            float[] ValuesAre = new float[4];
            ValuesAre[0] = PhysicalResistance[AtLevel];
            ValuesAre[1] = Piercing[AtLevel];
            ValuesAre[2] = HeatResistance[AtLevel];
            ValuesAre[3] = ExplosionResistance[AtLevel];
            return ValuesAre;
        }
    }
}
