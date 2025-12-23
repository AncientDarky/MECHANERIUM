using Mechaerium;
using System;
using System.Collections;
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

        [Header("Enemy combat Properties")]
        [SerializeField] RobiteData RobDataCombat;
        float HP;

        [Header("Compoenents")]
        NavMeshAgent Agent;
        Mecha Player;
        SphereCollider AttackRangeDetector, DetectionRange;
        Attacker AttackDetetion;
        Detector detector;
        [Header("Locked Crate Properties")]
        Lockedcrate TargetEvent;
        float DespawnAfterEventTimer;

        [Header("Radar Peroperties")]
        [SerializeField] GameObject Minimap_Icon;
        [SerializeField] float RadarDetectionResistance;


        public Action<GameObject> OnDeath;
        [SerializeField] float BaseLootMultiplier;
        [SerializeField] Cost[] BaseLoot;


        public Action OnIdle, OnPlayerEncounter, OnAttacking, OnStun, OnBurn, OnRoam;

        Coroutine RoamCorotine;
        Vector3[] RoamingPoints;
        int RoamingPointIndex;

        private void Awake()
        {
            Player = FindAnyObjectByType<Mecha>();
            Agent = GetComponent<NavMeshAgent>();

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
            Agent.speed = RobDataCombat.NormalSpeed;
            switch(RobType)
            {
                default:
                    OnRoam?.Invoke();
                    break;
                case RobiteTypes.SuddenRobite:
                    GoingAfterPlayer();
                    break;
                case RobiteTypes.CrateRobite:

                    break;
            }

        }
        public void Reinit(Lockedcrate Crate,Mecha Player)
        {
            if (Crate)
            {
                TargetEvent = Crate;
                AttackingCrate();
            }
        }
        private void OnEnable()
        {

            OnIdle += StartIdle;
            OnPlayerEncounter += StartPlayerEncounter;
            OnAttacking += StartAttacking;
            OnStun += StartStun;
            OnBurn += StartBurn;
            OnRoam += StartRoam;

        }
        private void OnDisable()
        {

            OnIdle -= StartIdle;
            OnPlayerEncounter -= StartPlayerEncounter;
            OnAttacking -= StartAttacking;
            OnStun -= StartStun;
            OnBurn -= StartBurn;
            OnRoam -= StartRoam;

        }
    
        public void Died()
        {
            if (0 == 0)
            {
                OnDeath?.Invoke(this.gameObject);
                this.gameObject.SetActive(false);
            }
        }
        void HandleState()
        {
            switch (state)
            {
                case RobiteStates.Idle:

                    break;
                case RobiteStates.Moving:

                    switch(RobType)
                    {
                        case RobiteTypes.SuddenRobite:

                            GoingAfterPlayer();

                            break;
                        case RobiteTypes.CrateRobite:

                            break;
                        default:

                            GoingAfterPlayer();

                            break;
                    }

                    break;
                case RobiteStates.Attacking:

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

                    break;
                case RobiteStates.Death:

                    break;
            }
        }

        private void GoingAfterPlayer()
        {
            Agent.SetDestination(Player.transform.position);

            Player.OnPlayerPositionChanged += PlayerChangedPosition;

        }
        private void AttackingCrate()
        {
            Agent.SetDestination(TargetEvent.transform.position);

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
    }

}

