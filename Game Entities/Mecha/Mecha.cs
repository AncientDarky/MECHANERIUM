using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Mechaerium
{
    public struct Damage
    {
        public float Physical, Piercing, Heat, Explosion;
    }
    public class Mecha : MonoBehaviour
    {
       public List<Module>MechaModules = new List<Module>();
        public float RemainingHealPoints { get { float Summ = 0; foreach (Module module in MechaModules) { Summ += module.HITPOINT; } return Summ; } }
        public float MaximumHealthPoints { get { float Summ = 0; foreach (Module module in MechaModules) { Summ += module.MAXHP; } return Summ; } }

        private void Start()
        {
            MechaModules.AddRange(this.gameObject.GetComponents<Module>());
        }
        public void TakeDamage(Damage damage)
        {
            ArmourModule Armour = GetComponent<ArmourModule>();
           
            float PhysicalResistance = 0;
            float HeatResistance = 0;
            float ExplosionResistance = 0;

            if (Armour)
            {
                 PhysicalResistance = Armour.PHSICALRESIST;
                 HeatResistance = Armour.HEATRESIST;
                 ExplosionResistance = Armour.EXPLORESIST;
            }
            if(GetComponent<RadarModule>())
            {

               GetComponent<RadarModule>().ReduceHP (damage.Physical - damage.Physical * PhysicalResistance);
            }
            else if (GetComponent<CPUModule>())
            {

                GetComponent<CPUModule>().ReduceHP(damage.Physical - damage.Physical * PhysicalResistance);
            }
            else if (GetComponent<ArmourModule>())
            {

                GetComponent<ArmourModule>().ReduceHP(damage.Physical - damage.Physical * PhysicalResistance);
            }
            else if (GetComponent<CoreModule>())
            {

                GetComponent<RadarModule>().ReduceHP(damage.Physical - damage.Physical * PhysicalResistance);
            }

            if (GetComponent<RadarModule>()) // Explo Damage
            {

                GetComponent<RadarModule>().ReduceHP(damage.Explosion - damage.Explosion * ExplosionResistance);
            }
            else if (GetComponent<CPUModule>())
            {

                GetComponent<CPUModule>().ReduceHP(damage.Explosion - damage.Explosion * ExplosionResistance);
            }
            else if (GetComponent<ArmourModule>())
            {

                GetComponent<ArmourModule>().ReduceHP(damage.Explosion - damage.Explosion * ExplosionResistance);
            }
            else if (GetComponent<CoreModule>())
            {

                GetComponent<RadarModule>().ReduceHP(damage.Explosion - damage.Explosion * ExplosionResistance);
            }

            if (GetComponent<RadarModule>()) // Heat Damage
            {

                GetComponent<RadarModule>().ReduceHP(damage.Heat - damage.Heat * HeatResistance);
            }
            else if (GetComponent<CPUModule>())
            {

                GetComponent<CPUModule>().ReduceHP(damage.Heat - damage.Heat * HeatResistance);
            }
            else if (GetComponent<ArmourModule>())
            {

                GetComponent<ArmourModule>().ReduceHP(damage.Heat - damage.Heat * HeatResistance);
            }
            else if (GetComponent<CoreModule>())
            {

                GetComponent<RadarModule>().ReduceHP(damage.Heat - damage.Heat * HeatResistance);
            }
        }

    }
}
