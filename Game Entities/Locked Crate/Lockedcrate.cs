using FischlWorks_FogWar;
using Mechaerium;

using Robitnekics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockedcrate : MonoBehaviour
{
    [SerializeField] int MaxHP;
    float HP;
    public CrateStates state;
    Storage MechaStorageInstance => Mecha.STORERAGE;
    [Header("Crate Hacking Properties")]
    [SerializeField] float RequiredHackingValue, CurrentHackingProgress, ResetDureation;
    public float HACKPROGRESS => CurrentHackingProgress;
    public float MAXPROGRESS => RequiredHackingValue;
    public bool HACKCOMPLETED => CurrentHackingProgress > RequiredHackingValue;
    public bool IsInitiated;
    Coroutine ResetCoro;

    public Action OnDestruction, OnHacked, OnHackStarted, OnReset;


    [SerializeField] Cost[] Loot;

    Animator animator;


    public List<Robites> SpawnedRobit = new List<Robites>();
    [Header("Enemy spawn Properties")]
    bool shouldSpawn;
    public Vector3[] SpawnLocations;
    [SerializeField] float MinSpawnDistance,MaxSpawnDistance;
    [SerializeField] int SpawnSpotCounts;
    [SerializeField] GameObject Robite_SpawnPrefabs;
    [SerializeField] float SpawningFrequency,SpawningCount;

    Coroutine SpawningCorotine;
    private void Start()
    {
        HP = MaxHP;
        state = CrateStates.Idle;
        animator = GetComponent<Animator>();

        SettingRobiterSpawnLocations();
    }

    private void SettingRobiterSpawnLocations()
    {
        SpawnLocations = new Vector3[SpawnSpotCounts];

        for (int a = 0; a < SpawnSpotCounts - 1; a++)
        {
            float XRandom = UnityEngine.Random.Range(transform.position.x + MinSpawnDistance, transform.position.x + MaxSpawnDistance);
            float ZRandom = UnityEngine.Random.Range(transform.position.z + MinSpawnDistance, transform.position.z + MaxSpawnDistance);

            int[] PosNeg = new[] { 1, -1 };

            SpawnLocations[a] = new Vector3(XRandom * PosNeg[UnityEngine.Random.Range(0, PosNeg.Length)], this.transform.position.y, ZRandom * PosNeg[UnityEngine.Random.Range(0, PosNeg.Length)]);

        }
    }
    void StartWave()
    {
        shouldSpawn = true;
        SpawningCorotine = StartCoroutine(SpawningRobites());
    }
    IEnumerator SpawningRobites()
    {
        while (shouldSpawn)
        {
            yield return new WaitForSeconds(SpawningFrequency);
            for(int i = 0; i < SpawningCount; i++)
            {

                Vector3 SpawnLocation = SpawnLocations[UnityEngine.Random.Range(0,SpawnLocations.Length - 1)];
                GameObject RobiteSpawned = Instantiate(Robite_SpawnPrefabs,SpawnLocation,Quaternion.identity);
                Robites Robiter = RobiteSpawned.GetComponent<Robites>();
                SpawnedRobit.Add(Robiter);
                Robiter.Reinit(this,false);

            }
        }
    }
    private void OnEnable()
    {
        OnDestruction += CrateDestroyed;
        OnHackStarted += StartWave;
    }
    private void OnDisable()
    {
        OnDestruction -= CrateDestroyed;
        OnHackStarted -= StartWave;
    }
    public void Hacking(float ProgressValue)
    {

        state = CrateStates.Hacking;

        CurrentHackingProgress += ProgressValue;
        if(CurrentHackingProgress >= RequiredHackingValue && animator.GetBool("IsHacked") == false)
        {
            CrateHacked();
        }
    }
    public void ResttingStarted()
    {

        state = CrateStates.Resetting;
        ResetCoro = StartCoroutine(ResetHackingProgress());
    }
    public void ResettingStopped()
    {
        if(ResetCoro == null)
        {
            return;
        }
        StopCoroutine(ResetCoro);

        state = CrateStates.Hacking;
    }
    void ReHacking()
    {

    }
    public void TakeDamage(Damage damage)
    {
        HP = Mathf.Clamp(HP - damage.Physical,0,MaxHP);
        HP = Mathf.Clamp(HP - damage.Piercing, 0, MaxHP);
        HP = Mathf.Clamp(HP - damage.Heat, 0, MaxHP);
        HP = Mathf.Clamp(HP - damage.Explosion, 0, MaxHP);

        if(HP <= 0)
        {
            OnDestruction?.Invoke();
        }
    }
    void CrateDestroyed()
    {
        foreach(Robites robiter in SpawnedRobit)
        {
            robiter.Reinit(null,true);
        }
        SpawnedRobit.Clear();

        shouldSpawn = false;
        StopAllCoroutines();
        state = CrateStates.Destroyed;
        animator.SetBool("IsDestroyed", true);
    }
    void CrateHacked()
    {

        shouldSpawn = false;
        StopAllCoroutines();
        state = CrateStates.Hacked;
        for(int i = 0; i < Loot.Length; i ++)
        {
            MechaStorageInstance.HarvestingMats(Loot[i].Material, Loot[i].Value);
            Debug.Log("Loot Received from locked Crate");

        }
        animator.SetBool("IsHacked",true);
    }
    IEnumerator ResetHackingProgress()
    {
        while(CurrentHackingProgress > 0)
        {
            yield return new WaitForSeconds(ResetDureation);

            state = CrateStates.Idle;
            CurrentHackingProgress = 0;
        }
    }
}
