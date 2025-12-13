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

        [SerializeField] Vector3 ProjectileVelocity;
        [SerializeField] float ProjectileSpeed;
        [SerializeField] float FireDelayBetweenShots;
        [SerializeField] float []ShellAccuracy;
        [SerializeField] float MaxFiringAngle;

        Transform CurrentCanon;

        private void Start()
        {

           

        }
        public void FireShell()
        {
            CurrentCanon = MortarCannons[Random.Range(0, MortarCannons.Length - 1)];

            Vector3 TargetLocation = MouseWorldLocationCurrent;
            Vector3 TargetWithAccurcy = TargetLocation + ProjectileAccurcy();
            Vector3 ProjectileDir = TargetWithAccurcy - CurrentCanon.position;

            GameObject NewShell = Instantiate(Shell, TargetWithAccurcy,Quaternion.identity);
            Debug.Log("FiredShell");

        }
        public void NotFiring()
        {
            WeaponStopFiring();

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
