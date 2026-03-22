using Robitnekics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Mechaerium
{
    public class RadarModule : Module
    {
        [Header("Detection Properties")]
        [SerializeField] float[] RadarStrength;
        [SerializeField] float[] RadarRange;
        [SerializeField] float[] RadarCooldown;

        [Header("Radar UI")]
        [SerializeField] TextMeshProUGUI[] Warnings = new TextMeshProUGUI[3];
        [SerializeField] Color NodeWarningColor, CrateWarningColor, RobiterWarningColor, InactiveColor;

        SphereCollider DetectionColli;
        ObjectDetector RadarDetect;
        Coroutine RadarCorotine;

        List<Robites> RobiterWithinRange = new List<Robites>(); 
        List<Robites> DetectedRobites = new List<Robites>();

        List<Node> NodeWithinRange = new List<Node>();
        List<Lockedcrate> CrateWithinRange = new List<Lockedcrate>();



        private void Start()
        {
            Invulnerable = false;
            GameObject NewGameColli = new GameObject("ObjectDetector_Radar");
            NewGameColli.transform.SetParent(this.transform);
            NewGameColli.transform.position = this.transform.position;
            DetectionColli = NewGameColli.AddComponent<SphereCollider>();
            RadarDetect = NewGameColli.AddComponent<ObjectDetector>();

            ObjectDetector.DetectionTarget[] TargetsToDetect = new ObjectDetector.DetectionTarget[] 
            { ObjectDetector.DetectionTarget.Node, 
              ObjectDetector.DetectionTarget.LockedCrate,
              ObjectDetector.DetectionTarget.Enemy }; 

            RadarDetect.Init(TargetsToDetect);

            RadarDetect.OnGameObjectDetected += OnTargetEnteredRadar;
            RadarDetect.OnGameObjectExited += OnTargetExitedRadar;

            DetectionColli.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            DetectionColli.isTrigger = true;
            DetectionColli.radius = RadarRange[ModuleLevel];

            RadarCorotine = StartCoroutine(RadarTick());

            OnRepaired += RestartRadarFunctionality;

            DeactivateWarningUI(0);
            DeactivateWarningUI(1);
            DeactivateWarningUI(2);
        }

        private void OnEnable()
        {
        }
        private void OnDisable()
        {
            RadarDetect.OnGameObjectDetected -= OnTargetEnteredRadar;
            RadarDetect.OnGameObjectExited -= OnTargetExitedRadar;

        }
        void DeactivateWarningUI(int A)
        {
            Warnings[A].color = InactiveColor;
        }
        void ActivateWarningUI(int A)
        {
            switch(A)
            {
                case 0:

                    Warnings[A].color = NodeWarningColor;
                    break;
                    case 1:

                    Warnings[A].color = CrateWarningColor;
                    break;
                    case 2:

                    Warnings[A].color = RobiterWarningColor;
                    break;
            }

        }
        private void OnTargetEnteredRadar(GameObject other)
        {
            if (other.gameObject.tag == "Enemy" && RobiterWithinRange.Contains(other.gameObject.GetComponent<Robites>()) == false)
            {
                RobiterWithinRange.Add(other.gameObject.GetComponent<Robites>());
            }
            else if (other.gameObject.tag == "Node" && NodeWithinRange.Contains(other.gameObject.GetComponent<Node>()) == false)
            {
                NodeWithinRange.Add(other.gameObject.GetComponent<Node>());
                ActivateWarningUI(0);
            }
            else if (other.gameObject.tag == "LockedCrate" && CrateWithinRange.Contains(other.gameObject.GetComponent<Lockedcrate>()) == false)
            {
                CrateWithinRange.Add(other.gameObject.GetComponent<Lockedcrate>());
                ActivateWarningUI(1);

            }
        }
        private void OnTargetStay(GameObject other)
        {
            if(other.gameObject.tag == "Enemy" && RobiterWithinRange.Contains(other.gameObject.GetComponent<Robites>()) == false)
            {
                RobiterWithinRange.Add(other.gameObject.GetComponent<Robites>());
            }
            else if (other.gameObject.tag == "Node" && NodeWithinRange.Contains(other.gameObject.GetComponent<Node>()) == false)
            {
                NodeWithinRange.Add(other.gameObject.GetComponent<Node>());
                ActivateWarningUI(0);
            }
            else if (other.gameObject.tag == "LockedCrate" && CrateWithinRange.Contains(other.gameObject.GetComponent<Lockedcrate>()) == false)
            {
                CrateWithinRange.Add(other.gameObject.GetComponent<Lockedcrate>());
                ActivateWarningUI(1);

            }
        }
        private void OnTargetExitedRadar(GameObject other)
        {
            if (other.gameObject.tag == "Enemy" && RobiterWithinRange.Contains(other.gameObject.GetComponent<Robites>()) == true)
            {
                RobiterWithinRange.Remove(other.gameObject.GetComponent<Robites>());
                other.gameObject.GetComponent<Robites>().OutofRadarRange();
                DisablingWarningIfConditionMet(other.gameObject.GetComponent<Robites>());
            }
            else if (other.gameObject.tag == "Node" && NodeWithinRange.Contains(other.gameObject.GetComponent<Node>()) == true)
            {
                NodeWithinRange.Remove(other.gameObject.GetComponent<Node>());
                if(NodeWithinRange.Count <= 0)
                {
                    DeactivateWarningUI(0);
                }
            }
            else if (other.gameObject.tag == "LockedCrate" && CrateWithinRange.Contains(other.gameObject.GetComponent<Lockedcrate>()) == true)
            {
                CrateWithinRange.Remove(other.gameObject.GetComponent<Lockedcrate>());
                if (CrateWithinRange.Count <= 0)
                {
                    DeactivateWarningUI(1);
                }
            }
        }

        private void DisablingWarningIfConditionMet(Robites other)
        {
            if (DetectedRobites.Contains(other))
            {
                DetectedRobites.Remove(other);
                if (DetectedRobites.Count <= 0)
                {
                    DeactivateWarningUI(2);
                }
            }
        }

        void RestartRadarFunctionality()
        {
            RadarCorotine = StartCoroutine(RadarTick());
        }
        IEnumerator RadarTick()
        {
            while(ModuleState != ModuleStates.Destroyed)
            {
                yield return new WaitForSeconds(RadarCooldown[ModuleLevel]);
                foreach(Robites robiter in RobiterWithinRange)
                {
                   if(robiter.DetectedByPlayer())
                    {
                        DetectedRobites.Add(robiter);
                        ActivateWarningUI(2);
                    }
                    else
                    {
                        DisablingWarningIfConditionMet(robiter);
                    }
                }

            }
        }
        #region UI Character Sheet 
        public float[] TransferModuleValues(int Index)
        {
            if (Index > MaxModuleLevel)
            {
                Index = MaxModuleLevel;
            }
            float[] TransferValues = new float[3];
            TransferValues[0] = RadarStrength[Index];
            TransferValues[1] = RadarRange[Index];
            TransferValues[2] = RadarCooldown[Index];


            return TransferValues;
        }
        #endregion
    }
}
