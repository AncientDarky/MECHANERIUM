using FischlWorks_FogWar;
using UnityEngine;
namespace Mechaerium
{


    public class Flamethrower : WeaponModule
    {
        [Header("Flame Thrower Properties")]
        [SerializeField] float[] Tick;
        [SerializeField] float[] BurnDuration;
        [SerializeField] Damage[] BurnDamage;
        [SerializeField] HitEffects OnBurnEffects;
        [SerializeField] LayerMask Targtables;

        private void Start()
        {
            WeaponAnimator.SetFloat("FireRate", Tick[ModuleLevel]);
            Visuals.SetBool("Toggle",false);
            Visuals.SetFloat("FlameRange", AutoEngageRange[ModuleLevel]);

            Hitpoint = MAXHP;
        }
        private void LateUpdate()
        {
            if(CurrentTarget)
            {
                Visuals.SetVector3("Direction",( CurrentTarget.transform.position - this.transform.position).normalized);
            }
        }
        public void FlameTick()
        {
            if(CurrentTarget)
            {
                RaycastHit[] TargetsWithinRaycast = Physics.RaycastAll(this.transform.position, CurrentTarget.transform.position - this.transform.position, AutoEngageRange[ModuleLevel], Targtables);
                for (int i = 0; i < TargetsWithinRaycast.Length; i++)
                {
                    Debug.DrawRay(this.transform.position, (CurrentTarget.transform.position - this.transform.position).normalized * AutoEngageRange[ModuleLevel], Color.red, 2);
                    Debug.Log("Burned " +
                    TargetsWithinRaycast[i].collider.gameObject.name);
                }
            }
          

        }
    }
}
