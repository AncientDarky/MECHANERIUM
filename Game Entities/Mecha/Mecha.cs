using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Mechaerium
{
    [Serializable]
    public struct Damage
    {
        public float Physical, Piercing, Heat, Explosion;
    }
    [RequireComponent(typeof(CharacterController))]

    public class Mecha : MonoBehaviour
    {
        [SerializeField]MechaStates state;

        public bool TestingHealth;
       public List<Module>MechaModules = new List<Module>();
        public float RemainingHealPoints { get { float Summ = 0; foreach (Module module in MechaModules) { Summ += module.HITPOINT; } return Summ; } }
        public float MaximumHealthPoints { get { float Summ = 0; foreach (Module module in MechaModules) { Summ += module.MAXHP; } return Summ; } }
        [Header("Player Input Action")]
        [SerializeField] InputActionReference Movement;
        [SerializeField] InputActionReference LMB;
        [SerializeField] InputActionReference LeftShift;
        [SerializeField] InputActionReference RMB;
        [SerializeField] InputActionReference Scanner;
        [SerializeField] InputActionReference MechaSheet;
        [SerializeField] InputActionReference ToggleAlt;
        [SerializeField] InputActionReference WeaponSlot1,WeaponSlot2,WeaponSlot3,WeaponSlot4,WeaponSlot5;

        [Header("Meha components")]
        CharacterController Controller;
        Animator LeggingAnimator;
        #region Movement Properties 
        [SerializeField] float NormalSpeed,RunningSpeed,MovementMultiplier;
        Vector3 MovementDirection;

        #endregion
        #region AimingDirection
        [SerializeField] Transform UpperSection,Leggings;
        Vector3 LookingDirection;
        
        RaycastHit MouseworldPoint;
        Ray ray;
        [SerializeField] LayerMask GroundLayer;
        #endregion
        #region Mecha Camera
        [SerializeField] CinemachineCamera CinemachineCamera;
        CameraTarget CurrentTarget;
        #endregion
        #region Mouse position in world 
        public Vector3 MouseWorldPosition;
        #endregion
        Storage MechStorage;
        public static Storage STORERAGE { get { if (FindAnyObjectByType<Mecha>().MechStorage != null) { return FindAnyObjectByType<Mecha>().MechStorage; } FindAnyObjectByType<Mecha>().MechStorage = new Storage(); return FindAnyObjectByType<Mecha>().MechStorage; } }

        public List<WeaponModule>Arsenal = new List<WeaponModule>();




        Damage dama;

        float t;

        private void OnEnable()
        {

         LeftShift.action.Enable();
         LeftShift.action.started += LeftShifting;
         LeftShift.action.canceled += LeftShifting;

         LMB.action.Enable();
         LMB.action.started += FiringManualWeapons;
         LMB.action.canceled += FiringManualWeapons;
       
        }
        private void OnDisable()
        {

         LMB.action.Disable();
         LMB.action.started -= FiringManualWeapons;
         LMB.action.canceled -= FiringManualWeapons;

         LeftShift.action.Disable();
         LeftShift.action.started -= LeftShifting;
         LeftShift.action.canceled -= LeftShifting;
       
        }
        private void Start()
        {
            CurrentTarget = new CameraTarget();
            CurrentTarget.TrackingTarget = this.gameObject.transform;
            CinemachineCamera.Target = CurrentTarget;

            MechaModules.AddRange(this.gameObject.GetComponents<Module>());
            dama = new Damage();
            dama.Physical = 10;

            Controller = GetComponent<CharacterController>();
            LeggingAnimator = Leggings.GetComponent<Animator>();
            MechStorage = new Storage();

            ChangeMechaState(MechaStates.Idle);
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
            Debug.Log("AAAAD" );

            for (int a = 5; a > 0;a--)
            {
                if (MechaModules[a].Invulnerable)
                {
                    continue;
                }
                Debug.Log("AAAAC" + a);
              
                    Debug.Log("AAAA");
                    if(MechaModules[a].enabled)
                    {
                        MechaModules[a].ReduceHP(dama.Physical - dama.Physical * PhysicalResistance); 
                        MechaModules[a].ReduceHP(dama.Heat - dama.Heat * HeatResistance);
                        MechaModules[a].ReduceHP(dama.Explosion - dama.Explosion * ExplosionResistance);
                    break;

                    }
                
            }
        }

        private void Update()
        {

            if (Movement.action.ReadValue<Vector2>().x != 0 || Movement.action.ReadValue<Vector2>().y != 0)
            {
                if(MovementMultiplier != GetComponent<ReactorModule>().RunningSpeed[GetComponent<ReactorModule>().ModuleLevel])
                {

                    ChangeMechaState(MechaStates.Walking);

                }
                Move();
                RotatingMechaTowardMovementDirection();
            }
            else
            {
                ChangeMechaState(MechaStates.Idle);
            }
            AimingAtDiretion();

            foreach (WeaponModule Weapon in Arsenal)
            {
                if ( Weapon.ToggleOn)
                {
                    Weapon.MouseWorldLocationCurrent = MouseWorldPosition;
                }
            }
        }
        void Move()
        {
            Debug.Log("Testing Movement");
            MovementDirection = new Vector3(Movement.action.ReadValue<Vector2>().x * MovementMultiplier, 0, Movement.action.ReadValue<Vector2>().y * MovementMultiplier);

            Controller.Move(MovementDirection);
        }
        void RotatingMechaTowardMovementDirection()
        {
            Vector3 MovementDir = (Leggings.position + MovementDirection) - Leggings.position;
            Quaternion LookRotation = Quaternion.LookRotation(MovementDir);
            Leggings.transform.rotation = LookRotation;
        }
        void AimingAtDiretion()
        {

            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(ray.origin,ray.direction,out MouseworldPoint,1000,GroundLayer))
            {

                MouseWorldPosition = MouseworldPoint.point;
                LookingDirection = (new Vector3(MouseworldPoint.point.x, transform.position.y, MouseworldPoint.point.z) - UpperSection.position).normalized;
                Vector3 YawDir = LookingDirection;
                YawDir.y = 0;
                Quaternion LookRotation = Quaternion.LookRotation(YawDir);
                UpperSection.transform.rotation = Quaternion.Slerp(UpperSection.rotation, LookRotation, Time.deltaTime * GetComponent<ReactorModule>().AimingRotationSpeed[GetComponent<ReactorModule>().ModuleLevel]);

            }


        }
        void LeftShifting(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                ChangeMechaState(MechaStates.Running);
            }
            else if(context.canceled)
            {
                ChangeMechaState(MechaStates.Walking);
            }
        }
        void ChangeMechaState(MechaStates ToState)
        {
            state = ToState;

            HandleStates();
        }
        void HandleStates()
        {
            switch (state)
            {
                case MechaStates.Walking:

                    LeggingAnimator.SetTrigger("Moving");
                    LeggingAnimator.ResetTrigger("Idle");
                    MovementMultiplier = GetComponent<ReactorModule>().NormalSpeed[GetComponent<ReactorModule>().ModuleLevel];
                    LeggingAnimator.SetFloat("SpeedMulti", 1);



                    break;
                case MechaStates.Running:
                    MovementMultiplier = GetComponent<ReactorModule>().RunningSpeed[GetComponent<ReactorModule>().ModuleLevel];
                    LeggingAnimator.SetFloat("SpeedMulti", 2);

                    break;
                case MechaStates.Idle:
                    LeggingAnimator.SetFloat("SpeedMulti", 1);
                    LeggingAnimator.ResetTrigger("Moving");

                    LeggingAnimator.SetTrigger("Idle");
                    MovementMultiplier =  GetComponent<ReactorModule>().NormalSpeed[GetComponent<ReactorModule>().ModuleLevel];

                    break;
            }
        }

        void FiringManualWeapons(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                foreach (WeaponModule Weapon in Arsenal)
                {
                    if(Weapon.ManuallyControlled)
                    {
                        Weapon.WeaponStartFiring();
                    }
                }
            }
            else if(context.canceled)
            {
                foreach (WeaponModule Weapon in Arsenal)
                {
                    if (Weapon.ManuallyControlled)
                    {
                        Weapon.WeaponStopFiring();
                    }
                }
            }
               
        }

    }
}
