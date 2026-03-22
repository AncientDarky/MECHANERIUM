using Robitnekics;
using UnityEngine;

namespace Mechaerium
{

    public class Minigun : WeaponModule
    {

        [Header("Minigun Properties ")]

        [SerializeField] float[] FireRate;
        [SerializeField] float[] Accuracy;
        [SerializeField] float[] Range;
        [SerializeField] float[] ProjectileVelocity;
        [SerializeField] LayerMask TargetsLayer;

        Robites LastHitTarget;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            WeaponAnimator.SetFloat("FireRate", FireRate[ModuleLevel]);
            Visuals.SetFloat("FireRate", FireRate[ModuleLevel]);
            Visuals.SetBool("Toggle",false);

            Hitpoint = MaxHitpoint[ModuleLevel];
            WeaponEntity = this.gameObject;
        }
        
        public void FireMinigun()
        {
            for(int ii= 0;ii < MaterialConsumption.Length;ii++)
            {
                if (FindAnyObjectByType<Mecha>().MechStorage.CheckForMats(MaterialConsumption[ii].Material, MaterialConsumption[ii].Value) == false)
                {
                    WeaponStopFiring();
                    return;
                }
            }

            Vector3 FireDirection = this.transform.forward;
            Vector3 FireDirectionWithAccuracy = FireDirection + ProjectileAccurcy();



            Visuals.SetVector3("Direction", FireDirectionWithAccuracy * ProjectileVelocity[ModuleLevel]);
            Visuals.SetFloat("FireRate", FireRate[ModuleLevel]);
            Visuals.SetFloat("Velocity", ProjectileVelocity[ModuleLevel]);


            Visuals.SetFloat("LifeTime", 3f);

            RaycastHit TargetHit;
            if (Physics.Raycast(this.transform.position, (FireDirectionWithAccuracy) * Range[ModuleLevel], out TargetHit, TargetsLayer))
            {
                if(TargetHit.collider.gameObject.tag == "Enemy")
                {

                    Vector3 ProjectileDirection = TargetHit.collider.transform.position - this.transform.position;

                    Debug.DrawRay(this.transform.position, (FireDirectionWithAccuracy) * Range[ModuleLevel], Color.blue, 5);

                    float TimeUntilHit = Vector3.Distance(TargetHit.transform.position, this.transform.position) / ProjectileVelocity[ModuleLevel];
                    LastHitTarget = TargetHit.collider.gameObject.GetComponent<Robites>();
                    Visuals.SetFloat("LifeTime",TimeUntilHit);
                    Invoke("DamagedTarget",TimeUntilHit);
                    Debug.Log("Hit a valid target");
                }
                else
                {
                    Vector3 ProjectileDirection = transform.localEulerAngles.normalized;


                    Debug.DrawRay(this.transform.position, (FireDirectionWithAccuracy) * ProjectileVelocity[ModuleLevel], Color.red, 5);

                    float TimeUntilHit = Vector3.Distance(TargetHit.transform.position, this.transform.position) / ProjectileVelocity[ModuleLevel];

                    Debug.Log("HitNBOT VALID target");

                }
            }

        }
        void DamagedTarget()
        {
            LastHitTarget.OnDamaged?.Invoke(BaseDamage);
        }
        Vector3 ProjectileAccurcy()
        {
            Vector3 MinOffset = new Vector3(-1, -1, -1);
            Vector3 MaxOffset = new Vector3(1,1,1);

            float Accurcy = Accuracy[ModuleLevel];

            float X = Random.Range(MinOffset.x, MaxOffset.x);

            X = X - X * Accurcy;

            float Z = Random.Range(MinOffset.z, MaxOffset.z);
            Z = Z - Z * Accurcy;

            return new Vector3(X, 0, Z);


        }
       
    }
}
