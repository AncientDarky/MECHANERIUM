using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;
namespace Mechaerium
{


    public class Mortar : WeaponModule
    {
        [Header("Mortar Properties")]
        [SerializeField] Transform[] MortarCannons;
        [SerializeField] GameObject Shell;
        [SerializeField] float FlightTime;
        [SerializeField] float ProjectileSpeed;
        [SerializeField] float FireDelayBetweenShots;
        [SerializeField] float []ShellAccuracy;
        [SerializeField] float MaxFiringAngle;

        Transform CurrentCanon;

        private void Start()
        {

            Visuals.SetBool("Toggle",false);

        }
        public void FireShell()
        {
            CurrentCanon = MortarCannons[Random.Range(0, MortarCannons.Length - 1)];



            Vector3 TargetLocation = Mecha.MouseWorldPosition;
            Vector3 TargetWithAccurcy = TargetLocation + ProjectileAccurcy();
            Vector3 ProjectileDir = new Vector3( TargetWithAccurcy.x - CurrentCanon.position.x,0, TargetWithAccurcy.z - CurrentCanon.position.z);

            Vector3 Result = (TargetWithAccurcy - CurrentCanon.position - 0.5f * Physics.gravity * FlightTime * FlightTime) / FlightTime;

            Visuals.SetVector3("DirectionVelo", Result);
            Visuals.SetVector3("TargetLocation", new Vector3(TargetWithAccurcy.x, 0,TargetWithAccurcy.z));
            Visuals.SetBool("Toggle", true);


            GameObject NewShell = Instantiate(Shell, TargetWithAccurcy,Quaternion.identity);
            NewShell.GetComponent<ArtilleryShell>().Init(FlightTime);
            Debug.Log("FiredShell");

        }
        public void NotFiring()
        {
            WeaponStopFiring();
            Visuals.SetBool("Toggle", false);

        }
        Vector3 ProjectileAccurcy()
        {
            Vector3 MinOffset = new Vector3(-1 * AccuracyOffset.x, 0 * AccuracyOffset.y, -1 * AccuracyOffset.z);
            Vector3 MaxOffset = new Vector3(1 * AccuracyOffset.x, 0 * AccuracyOffset.y, 1 * AccuracyOffset.z);

            float Accurcy = ShellAccuracy[ModuleLevel];

            float X = Random.Range(MinOffset.x, MaxOffset.x);

            X = X - X * Accurcy;

            float Z = Random.Range(MinOffset.z, MaxOffset.z);
            Z = Z - Z * Accurcy;

            return new Vector3(X, 0, Z);
        }
      
    }
}
