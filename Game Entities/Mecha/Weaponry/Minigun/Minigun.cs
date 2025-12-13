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



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ManuallyControlled = true;
            WeaponAnimator.SetFloat("FireRate", FireRate[ModuleLevel]);
        }
        
        public void FireMinigun()
        {
            Vector3 FireDirection = this.transform.forward;

           

            RaycastHit TargetHit;
            if (Physics.Raycast(this.transform.position, (FireDirection + ProjectileAccurcy()) * Range[ModuleLevel], out TargetHit, TargetsLayer))
            {
                if(TargetHit.collider.gameObject.tag == "Enemy")
                {

                    Debug.DrawRay(this.transform.position, (FireDirection + ProjectileAccurcy()) * Range[ModuleLevel], Color.blue, 5);

                    float TimeUntilHit = Vector3.Distance(TargetHit.transform.position, this.transform.position) / ProjectileVelocity[ModuleLevel];

                    Debug.Log("Hit a valid target");
                }
                else
                {

                    Debug.DrawRay(this.transform.position, (FireDirection + ProjectileAccurcy()) * Range[ModuleLevel], Color.red, 5);

                    float TimeUntilHit = Vector3.Distance(TargetHit.transform.position, this.transform.position) / ProjectileVelocity[ModuleLevel];

                    Debug.Log("HitNBOT VALID target");

                }
            }

        }
        Vector3 ProjectileAccurcy()
        {
            Vector3 MinOffset = new Vector3(-1, -1, -1);
            Vector3 MaxOffset = new Vector3(1,1,1);

            float Accurcy = Accuracy[ModuleLevel];

            float X = Random.Range(MinOffset.x, MaxOffset.x);

            X = X - X * Accurcy;

            float Y = Random.Range(MinOffset.y, MaxOffset.y);
            Y = Y - Y * Accurcy;

            return new Vector3(X, Y, 0);


        }
    }
}
