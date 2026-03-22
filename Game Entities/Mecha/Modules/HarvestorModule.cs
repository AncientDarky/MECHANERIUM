using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mechaerium
{
    public class HarvestorModule : Module
    {
        [SerializeField] float[] GatheringYield;
        [SerializeField] float[] GatheringSpeed;
        [SerializeField] float[] GatheringDamage;
        [SerializeField] float[] GatheringRange;
        [SerializeField] int[] MaximumTargets;

        float YIELD => GatheringYield[ModuleLevel];
        float GATHERSPEED => GatheringSpeed[ModuleLevel];
        float GATHERRANGE => GatheringRange[ModuleLevel];
        float GATHERDAMAGE => GatheringDamage[ModuleLevel];
        int MAXTARGET => MaximumTargets[ModuleLevel];



        SphereCollider DetectorColli;
        ObjectDetector HarvestDetect;
        Coroutine HarvestingCoro;

        public List<Node>NodeList = new List<Node>();

        bool NodeWithinRange => NodeList.Count > 0;
        private void Awake()
        {
            GameObject NewChild = new GameObject("ObjectDetector_Harvestor");
            NewChild.transform.SetParent(this.gameObject.transform);
            NewChild.transform.position = this.transform.position;

            DetectorColli = NewChild.AddComponent<SphereCollider>();
            HarvestDetect = NewChild.AddComponent<ObjectDetector>();
            ObjectDetector.DetectionTarget[] TargetsToDetect = new ObjectDetector.DetectionTarget[]
            { ObjectDetector.DetectionTarget.Node};

            HarvestDetect.Init(TargetsToDetect);
            HarvestDetect.OnGameObjectDetected += OnNodeEncountered;
            HarvestDetect.OnGameObjectExited += OnNodeExited;
            DetectorColli.isTrigger = true;
            DetectorColli.radius = GATHERRANGE;

        }
        private void OnEnable()
        {


        }
        private void OnDisable()
        {

            HarvestDetect.OnGameObjectDetected -= OnNodeEncountered;
            HarvestDetect.OnGameObjectExited -= OnNodeExited;
        }
        private void OnNodeEncountered(GameObject other)
        {
            switch(other.gameObject.tag)
            {
                case "Node":
                    NodeList.Add(other.gameObject.GetComponent<Node>());

                    HandleStates();
                    break;
            }
        }
        private void OnNodeExited(GameObject other)
        {
            switch (other.gameObject.tag)
            {
                case "Node":
                    if(NodeList.Contains(other.gameObject.GetComponent<Node>()))
                    {
                        NodeList.Remove(other.gameObject.GetComponent<Node>());
                    }
                    HandleStates();
                    break;
            }
        }
        void HandleStates()
        {
            if(NodeList.Count <= 0)
            {
                StopCoroutine(HarvestingCoro);
                HarvestingCoro = null;
                // no working idle
            }
            else if(HarvestingCoro == null)
            {
                HarvestingCoro = StartCoroutine(HarvestorTick());
            }
        }
        void ConfigureStats()
        {

        }
        IEnumerator HarvestorTick()
        {
            while(NodeWithinRange)
            {
                yield return new WaitForSeconds(GATHERSPEED);
                for (int i = 0; i < MAXTARGET; i++)
                {
                         if(i >= NodeList.Count)
                         {
                        break;
                         }
                         if(MAXTARGET - 1 >= NodeList.Count - 1)
                    {
                        List<Node> list = new List<Node>();

                        foreach (Node c in NodeList)
                        {
                            if (c.HarvestedTicked(Mathf.RoundToInt(GATHERDAMAGE), YIELD))
                            {
                                list.Add(c);
                            }
                        }
                        foreach (Node c in list)
                        {
                            NodeList.Remove(c);
                        }
                        list.Clear();

                        break;
                    }
                    else
                    {
                        Debug.Log("B" + i);
                        if (NodeList[i].HarvestedTicked(Mathf.RoundToInt(GATHERDAMAGE), YIELD))
                        {
                            NodeList.RemoveAt(i);
                        }
                    }
                     
                    
                }

            }
        }

        #region UI Chartacter Sheet 
        public float[] TransferModuleValues(int Index)
        {
            if(Index > MaxModuleLevel)
            {
                Index = MaxModuleLevel;
            }
            float[] Values = new float[4];

            Values[0] = GatheringDamage[Index];
            Values[1] = GatheringYield[Index];
            Values[2] = GatheringRange[Index];
            Values[3] = MaximumTargets[Index];

            return Values;
        }
    }
        #endregion
    
}
