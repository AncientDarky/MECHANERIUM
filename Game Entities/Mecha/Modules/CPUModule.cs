using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
namespace Mechaerium
{

    public class CPUModule : Module
    {
        
        [Header("Orbital Hacking")]
        [SerializeField] float[] HackingSpeed;
        [SerializeField] float[] HackingRange;
        [SerializeField] float BaseSpeed;
        [SerializeField] float InitiationTime;

        [SerializeField] float LosingCompleteConnectionDuration; // it must BE SAME AS CRATE's RESET DURATION
        [Header("UI Elements")]
        [SerializeField] Image HackingBar,ConnectSymbol;
        [SerializeField] TextMeshProUGUI ProgressionTextBar;
        float CurrentDistanceToCrate;
        float CurrentProgression;
        float RequiredProgression;

        SphereCollider DetectionColli;
        ObjectDetector CPUDetect;
        Coroutine CPUCoro,ResetCoro,InitCoro;

        List<Lockedcrate> Crates = new List<Lockedcrate>();
        List<Lockedcrate> ResettingCrates = new List<Lockedcrate>();

        Lockedcrate CurrentlyHacking;



        private void Start()
        {
            Invulnerable = false;

            GameObject NewGameColli = new GameObject("ObjectDetector_CPU");
            NewGameColli.transform.SetParent(this.transform);
            NewGameColli.transform.position = this.transform.position;
            DetectionColli = NewGameColli.AddComponent<SphereCollider>();
            DetectionColli.radius = HackingRange[ModuleLevel];
            DetectionColli.isTrigger = true;

            CPUDetect = NewGameColli.AddComponent<ObjectDetector>();
            ObjectDetector.DetectionTarget[] TargetsToDetect = new ObjectDetector.DetectionTarget[]
            {  ObjectDetector.DetectionTarget.LockedCrate };

            CPUDetect.Init(TargetsToDetect);

            CPUDetect.OnGameObjectDetected += OnCrateEntered;
            CPUDetect.OnGameObjectExited += onCrateExited;
            HackingBar.fillAmount = 0;
            ConnectSymbol.fillAmount = 0;
            ProgressionTextBar.text = "0 %";
          
            
        }
        private void OnDisable()
        {
            CPUDetect.OnGameObjectDetected -= OnCrateEntered;
            CPUDetect.OnGameObjectExited -= onCrateExited;
        }
        IEnumerator CPUTick()
        {
            while (Crates.Count > 0)
            {
                yield return new WaitForSeconds(1);


                if (Crates.Count > 0)
                {

                    DisplayingConnectionStrength();
                    DisplayingHackingProgress();

                    if (CurrentlyHacking.HACKCOMPLETED)
                    {

                        Crates.RemoveAt(0);

                        StopCoroutine(CPUCoro);

                        HackingRemainingCrate();

                    }
                    else if(CurrentlyHacking.state == CrateStates.Destroyed)
                    {

                        Crates.RemoveAt(0);

                        StopCoroutine(CPUCoro);

                        HackingRemainingCrate();
                    }
                    else
                    {

                        Debug.Log("Hacking " + Crates[0].gameObject.name);

                        Crates[0].Hacking(BaseSpeed * HackingSpeed[ModuleLevel]);

                        DisplayingConnectionStrength();
                        DisplayingHackingProgress();
                    }

                }

            }
        }

        private void HackingRemainingCrate()
        {
            if (Crates.Count >= 1)
            {
                Debug.Log("Multiple Crates  Init");


                if (Crates[0].IsInitiated)
                {

                    Debug.Log("Ís Already Initied skipping init coro");
                    Crates[0].ResettingStopped();
                    CurrentlyHacking = Crates[0];
                    CPUCoro = StartCoroutine(CPUTick());
                }
                else if (Crates[0].IsInitiated == false)
                {

                    CurrentlyHacking = Crates[0];
                    Debug.Log("Ís NOT Already Initied starting InitCoro");
                    StartCoroutine(InitiatingHack());
                }
            }
        }

        private void DisplayingHackingProgress()
        {
            if(CurrentlyHacking)
            {
                HackingBar.fillAmount = CurrentlyHacking.HACKPROGRESS / CurrentlyHacking.MAXPROGRESS;
                ProgressionTextBar.text = (Mathf.Clamp(CurrentlyHacking.HACKPROGRESS / CurrentlyHacking.MAXPROGRESS * 100, 0, 100)).ToString() + " %";

            }
         }
        void ConnectionLost()
        {
            HackingBar.color = Color.yellow;
            ConnectSymbol.color = Color.yellow;
        }
        void ConnectionCompletelyLost()
        {
            ConnectSymbol.fillAmount = 0;
            HackingBar.fillAmount = 0;
        }
        void OnCrateEntered(GameObject Crate)
        {
            if(Crate.GetComponent<Lockedcrate>())
            {
                Lockedcrate EnteredCrate = Crate.GetComponent<Lockedcrate>();
                if (EnteredCrate.HACKCOMPLETED) { return;  }

                Crates.Add(EnteredCrate);
                if (Crates.Count == 1)
                {
                    HackingBar.color = Color.green;
                    ConnectSymbol.color = Color.green;

                    Debug.Log("First Time Init");
                    if(EnteredCrate.IsInitiated)
                    {
                        CancelInvoke("ConnectionCompletelyLost");
                        Debug.Log("Crate Already Initialed continued Hacking");
                        EnteredCrate.ResettingStopped();

                        CPUCoro = StartCoroutine(CPUTick());
                    }
                    else if (EnteredCrate.IsInitiated == false)
                    {
                        CurrentlyHacking = Crates[0];

                        StartCoroutine(InitiatingHack());
                    }
                }

                // Initialize First if 
                // Crate was not init 
                //Skip inittialze if 
                // If Crate was Init still and has some Hackingprogress so stop Reset Corotine in lockedCrate


            }
        }

        private void DisplayingConnectionStrength()
        {
            if (Crates.Count > 0)
            {
                float DistanceToCrate = HackingRange[ModuleLevel] - Vector3.Distance(Crates[0].gameObject.transform.position, this.transform.position);
                ConnectSymbol.fillAmount = DistanceToCrate / HackingRange[ModuleLevel];
            }
        }

        public IEnumerator InitiatingHack()
        {
           
                yield return new WaitForSeconds(InitiationTime);
                


                // Locked crate starts spawning enemies and will not stop until locked crate is hacked or destroyed
                // once destroyed or hacked any enemy spawned by Crate will turn into Standard enemy

                Debug.Log("InitiatingA");
                CurrentlyHacking.IsInitiated = true;
                DisplayingConnectionStrength();
                CurrentlyHacking.OnHackStarted?.Invoke();
                CPUCoro = StartCoroutine(CPUTick());
        }
        void onCrateExited(GameObject Crate)
        {
            if (Crate.GetComponent<Lockedcrate>())
            {
                Lockedcrate CrateExited = Crate.GetComponent<Lockedcrate>();

                if(Crates.Contains(CrateExited))
                {
                    Crates.Remove(CrateExited);
                    if(CrateExited.HACKPROGRESS > 0 && CrateExited.HACKCOMPLETED == false)
                    {
                        CrateExited.ResttingStarted();

                       
                    }
                    if(Crates.Count > 0)
                    {
                        HackingBar.color = Color.green;
                        ConnectSymbol.color = Color.green;
                        DisplayingConnectionStrength();

                        if (Crates[0].IsInitiated)
                        {
                            Debug.Log("Crate Already Initialed continued Hacking");
                            Crates[0].ResettingStopped();
                            CPUCoro = StartCoroutine(CPUTick());
                        }
                        else if (Crates[0].IsInitiated == false)
                        {
                            StartCoroutine(InitiatingHack());
                        }
                    }
                    else
                    {
                        ConnectionLost();
                        Invoke("ConnectionCompletelyLost",LosingCompleteConnectionDuration);
                        StopAllCoroutines();
                        DisplayingConnectionStrength();
                       
                    }
                }
               // if was Init and progress was not completed Start Reseting corotine for locked crate
               // Remove Crate from Crates List 
            }
        }
        #region UI Chartacter Sheet 
        public float [] TransferModuleValues(int Index)
        {
            if (Index > MaxModuleLevel)
            {
                Index = MaxModuleLevel;
            }
            float[] Values = new float[2];

            Values[0] = HackingSpeed[Index];
            Values[1] = HackingRange[Index];

            return Values;
        }
    }
        #endregion
    
}
