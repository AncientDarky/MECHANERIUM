using Mechaerium;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Robitnekics
{
    public class Robites : MonoBehaviour
    {

        [Header("States")]
        [SerializeField] RobiteStates state;
        [SerializeField] RobiteTypes RobType;
        public RobiteTypes ROBTYPE => RobType;
        [Header("Enemy combat Properties")]
        [SerializeField] RobiteData RobDataCombat;
        public float HP;
        [SerializeField] GameObject Loot;
        [Header("Compoenents")]
        NavMeshAgent Agent;
        Mecha Player;
        SphereCollider AttackRangeDetector, DetectionRange;
        Attacker AttackDetetion;
        Detector detector;
        Animator animator;
        [Header("Locked Crate Properties")]
        Lockedcrate TargetEvent;
        GameObject AttackingTarget;
        float DespawnAfterEventTimer;

        [Header("Radar Peroperties")]
        [SerializeField] GameObject Minimap_Icon;
        [SerializeField] float RadarDetectionResistance;


        public Action<GameObject> OnDeath;
        [SerializeField] float BaseLootMultiplier;
        [SerializeField] Cost[] BaseLoot;


        public Action OnIdle, OnEngagement, OnAttacking, OnStun, OnBurn, OnRoam;

        public Action<Damage> OnDamaged;

        Coroutine RoamCorotine;
        Coroutine AttackCorotine;


        Vector3[] RoamingPoints;
        int RoamingPointIndex;

        private void Awake()
        {
            Player = FindAnyObjectByType<Mecha>();
            Agent = GetComponent<NavMeshAgent>();

            animator = GetComponent<Animator>();


            state = RobiteStates.Idle;

            AttackRangeDetector = this.transform.Find("AttackRange").gameObject.GetComponent<SphereCollider>();
            AttackRangeDetector.radius = RobDataCombat.AttackRange;


            DetectionRange = this.transform.Find("Detection").gameObject.GetComponent<SphereCollider>();

            DetectionRange.radius = RobDataCombat.PlayerDetectionRange;

            DetectionRange.isTrigger = true;
            AttackRangeDetector.isTrigger = true;
            DetectionRange.AddComponent<Detector>();
            AttackRangeDetector.AddComponent<Attacker>();
            AttackDetetion = AttackRangeDetector.GetComponent<Attacker>();
            detector = DetectionRange.GetComponent<Detector>();
            AttackDetetion.Owner = this;
            detector.Owner = this;
            RoamingPoints = new Vector3[3];


        }


        private void Start()
        {
           


            Minimap_Icon.SetActive(false);
            Agent.speed = RobDataCombat.NormalSpeed;
            animator.SetFloat("AttackspeedMultipler", 1 / RobDataCombat.Attackspeed);
            animator.SetFloat("MovementSpeedMultiplier", RobDataCombat.NormalSpeed);

            HP = RobDataCombat.MaxHP;

            switch (RobType)
            {
                default:
                    OnRoam?.Invoke();
                    break;
                case RobiteTypes.SuddenRobite:
                    GoingAfterPlayer();
                    break;
                case RobiteTypes.CrateRobite:
                    OnEngagement?.Invoke();
                    break;
            }


        }
        public void Reinit(Lockedcrate Crate,bool StandardOrSudden)
        {
            if(Agent)
            {
                Agent.ResetPath();
            }

            if (Crate)
            {
                TargetEvent = Crate;
                RobType = RobiteTypes.CrateRobite;
                OnEngagement?.Invoke();
            }
            else if(StandardOrSudden)
            {
                TargetEvent = null;
                RobType = RobiteTypes.Standard;
                OnEngagement?.Invoke();
            }
            else if (StandardOrSudden == false)
            {

                RobType = RobiteTypes.SuddenRobite;
                OnEngagement?.Invoke();
            }
        }
        private void OnEnable()
        {

            OnIdle += StartIdle;
            OnEngagement += StartPlayerEncounter;
            OnAttacking += StartAttacking;
            OnStun += StartStun;
            OnBurn += StartBurn;
            OnRoam += StartRoam;
            OnDamaged += DamageReceived;

        }
        private void OnDisable()
        {

            OnIdle -= StartIdle;
            OnEngagement -= StartPlayerEncounter;
            OnAttacking -= StartAttacking;
            OnStun -= StartStun;
            OnBurn -= StartBurn;
            OnRoam -= StartRoam;
            OnDamaged -= DamageReceived;

        }
        void DamageReceived(Damage damage)
        {
            // calculate Resistance 

            float IncomingPhysicalDamage = damage.Physical - damage.Physical *  RobDataCombat.PhysicalResist;
            float IncomingPiercingDamage = damage.Piercing - damage.Piercing *  RobDataCombat.PiercResist;
            float IncomingHeatDamage = damage.Heat - damage.Heat * RobDataCombat.HeatResist;
            float IncomingExplosiveDamage = damage.Explosion - damage.Explosion * RobDataCombat.ExploResist;



            // calculate effects Chance triggering 
           
            

            // applying damage 

            HP -= IncomingPhysicalDamage;
            HP -= IncomingPiercingDamage;
            HP -= IncomingHeatDamage;
            HP -= IncomingExplosiveDamage;

            // check for remaining HP 
            if(HP > 0)
            {
                Debug.Log("Damage Received but still alive" + HP);
                switch(RobType)
                {
                    case RobiteTypes.SuddenRobite:
                        break;
                        case RobiteTypes.CrateRobite:
                        GoingAfterPlayer();
                        break;
                    default:
                        break;
                }
                return;

            }
            Died();
            // Dropping Loots 
            if(Loot)
            {
                Instantiate(Loot, this.transform.position, Quaternion.identity);
            }
            // Disabling Corotines & Scripts 
            
            
        }
        void Died()
        {
            state = RobiteStates.Death;
            HandleState();
            OnDeath?.Invoke(this.gameObject);

        }
        public void RemovingEntitiy()
        {
            StopAllCoroutines();
            // Removing Game Entity
            this.enabled = false;
            Destroy(this.gameObject);
        }
        void HandleState()
        {
            switch (state)
            {
                case RobiteStates.Idle:

                    break;
                case RobiteStates.Moving:// must know when Crate start chasing player again if it was damaged by player 

                    switch(RobType)
                    {
                        case RobiteTypes.SuddenRobite:

                            GoingAfterPlayer();

                            break;
                        case RobiteTypes.CrateRobite:
                            if(AttackingTarget == Player.gameObject)
                            {
                                GoingAfterPlayer();
                                return;
                            }
                            UnsubribeFromPlayerLocationSignal();
                            AttackingCrate();

                            break;
                        default:
                            if(Vector3.Distance(Player.gameObject.transform.position,this.gameObject.transform.position) <= DetectionRange.radius / 2)
                            {

                                GoingAfterPlayer();
                            }
                            else
                            {
                                OnRoam?.Invoke();
                            }

                            break;
                    }

                    ResetAnimatorBools();
                    animator.SetBool("IsMoving", true);

                    break;
                case RobiteStates.Attacking:
                    if(AttackingTarget)
                    {
                        
                        AttackCorotine = StartCoroutine(Attacking());

                        ResetAnimatorBools();
                        animator.SetBool("IsAttacking", true);
                        

                    }

                    break;
                case RobiteStates.Stunned:

                    break;
                    case RobiteStates.Roaming:
                    
                    switch (RobType)
                    {
                        case RobiteTypes.SuddenRobite:


                            break;
                        case RobiteTypes.CrateRobite:


                            break;
                        default:

                           for (int x = 0; x < RoamingPoints.Length;x++)
                            {
                                Vector3 startingPos = this.transform.position;
                                float Min = 2;
                                float Max = 4;
                                Vector3 NewPoint = new Vector3(UnityEngine.Random.Range(-Min,Max), 0, UnityEngine.Random.Range(-Min, Max));
                                Vector3 FinalPoint = startingPos + NewPoint;
                                RoamingPoints[x] = FinalPoint;
                            }
                            RoamingPointIndex = 0;
                            Agent.SetDestination(RoamingPoints[RoamingPointIndex]);
                            float TimeUntilReachingDestination = Vector3.Distance(RoamingPoints[RoamingPointIndex],this.transform.position) / Agent.speed;
                            RoamCorotine = StartCoroutine(RoamingArea());
                            break;
                    }

                    ResetAnimatorBools();
                    animator.SetBool("IsMoving", true);
                    break;
                case RobiteStates.Death:

                    ResetAnimatorBools();
                    animator.SetBool("IsDeath", true);

                    break;
            }
        }
        void ResetAnimatorBools()
        {
            animator.SetBool("IsAttacking",false);
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsDeath", false);

        }
        private void GoingAfterPlayer()
        {
            Agent.SetDestination(Player.transform.position);

            Player.OnPlayerPositionChanged += PlayerChangedPosition;

            AttackingTarget = Player.gameObject;

        }
        private void AttackingCrate()
        {
            Agent.SetDestination(TargetEvent.transform.position);

            AttackingTarget = TargetEvent.gameObject;
            UnsubribeFromPlayerLocationSignal();
        }
        void UnsubribeFromPlayerLocationSignal()
        {
            Player.OnPlayerPositionChanged -= PlayerChangedPosition;
        }
        void StartIdle()
        {

        }
        void StartAttacking()
        {

            state = RobiteStates.Attacking;
            UnsubribeFromPlayerLocationSignal();
            HandleState();

        }
        void StartPlayerEncounter()
        {
            state = RobiteStates.Moving;
            HandleState();
            Debug.Log("Starting to Chase Player");
        }
        void PlayerChangedPosition(Vector3 NewLocation)
        {
            if(Agent == null) { return; }
            // Check if typ is Standard or sudden or if it is attacked by player 
            Agent.destination = NewLocation;
            Debug.Log("Updated Path");
        }
        void StartStun()
        {

            state = RobiteStates.Stunned;
            HandleState();
        }
        void StartBurn()
        {

            HandleState();
        }
        void StartMoving()
        {
            state = RobiteStates.Moving;

            HandleState();
        }
        void StartDeath()
        {

            state = RobiteStates.Death;
            HandleState();
        }
        void StartRoam()
        {

            state = RobiteStates.Roaming;
            UnsubribeFromPlayerLocationSignal();
            HandleState();
        }
        void ReachedRoamingPoint()
        {
            if (state != RobiteStates.Roaming) { RoamingPointIndex = 0;  return; }
            Debug.Log("Invoke Called");
            RoamingPointIndex++;

            if (RoamingPointIndex >= RoamingPoints.Length)
            {
                RoamingPointIndex = 0;

                Agent.SetDestination(RoamingPoints[RoamingPointIndex]);
                float TimeUntilReachingDestination = Vector3.Distance(RoamingPoints[RoamingPointIndex], this.transform.position) / Agent.speed;
                Invoke("ReachedRoamingPoint", TimeUntilReachingDestination);
                return;
            }
            Agent.SetDestination(RoamingPoints[RoamingPointIndex]);
            float TimeuntilDestination = Vector3.Distance(RoamingPoints[RoamingPointIndex], this.transform.position) / Agent.speed;
            Invoke("ReachedRoamingPoint", TimeuntilDestination);
        }
        IEnumerator RoamingArea()
        {
            while(state == RobiteStates.Roaming)
            {
                yield return new WaitForSeconds(Vector3.Distance(RoamingPoints[RoamingPointIndex], this.transform.position) / Agent.speed);

                RoamingPointIndex++;
                if (RoamingPointIndex >= RoamingPoints.Length)
                {
                    RoamingPointIndex = 0;

                    Agent.SetDestination(RoamingPoints[RoamingPointIndex]);
                    
                }
                else
                {
                    Agent.SetDestination(RoamingPoints[RoamingPointIndex]);

                }
            }
        }
        IEnumerator Attacking()
        {
            while(state == RobiteStates.Attacking)
            {
                yield return new WaitForSeconds(RobDataCombat.Attackspeed);
                if(state != RobiteStates.Attacking)
                {
                    StopCoroutine(AttackCorotine);
                }
                switch(AttackingTarget.tag)
                {
                    case "LockedCrate":

                        TargetEvent.TakeDamage(RobDataCombat.BaseDamage);

                        break;
                    default:
                        Debug.Log("Attacked player");
                        Player.TakeDamage(RobDataCombat.BaseDamage);
                        break;
                }
            }
        }

        #region Radar Scann
        public bool DetectedByPlayer()
        {
            float DetectionChance = 1 - RadarDetectionResistance;
            int Random = UnityEngine.Random.Range(1,100);
            if (Minimap_Icon == null)
            {
                Debug.Log("Minimap does ntot exits");
                return false;
            }
            if(DetectionChance <= 0)
            {

                return false;
            }
            else if(Random <= (DetectionChance * 100))
            {
                Minimap_Icon.gameObject.SetActive(true);
                return true;
            }
            return false;
        }
        public void OutofRadarRange()
        {

            Minimap_Icon.gameObject.SetActive(false);
        }
        #endregion
    }

}

