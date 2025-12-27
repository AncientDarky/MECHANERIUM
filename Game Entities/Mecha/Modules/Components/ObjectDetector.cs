using UnityEngine;
using System.Collections.Generic;
using System;
public class ObjectDetector : MonoBehaviour
{
    public enum DetectionTarget
    {
        None = 0,
        Node = 1,
        LockedCrate = 2,
        Enemy = 3
    }
    public List<GameObject> DetectedObjects = new List<GameObject>();
    public Action<GameObject> OnGameObjectDetected, OnGameObjectExited;
    [SerializeField]DetectionTarget[] Detecttarget;
    
    public List<GameObject> GetDetectedobjects () { return DetectedObjects; }
    private void Start()
    {

    }
    public void Init(DetectionTarget[] target)
    {
        Detecttarget = new DetectionTarget[target.Length];
        Detecttarget = target;

    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {

        for (int i = 0; i < Detecttarget.Length;i++)
        {

            Debug.Log("Detectinfg " + Detecttarget[i].ToString()); 
            if (Detecttarget[i] == DetectionTarget.None)
            {
                continue;
            }
            if (other.gameObject.tag == Detecttarget[i].ToString())
            {
                Debug.Log("detected Correctly" + Detecttarget[i].ToString());
                OnGameObjectDetected?.Invoke(other.gameObject);
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < Detecttarget.Length; i++)
        {
            if (Detecttarget[i] == DetectionTarget.None)
            {
                continue;
            }
            if (other.gameObject.tag == Detecttarget[i].ToString())
            {
                Debug.Log("Undetected Correctly");

                OnGameObjectExited?.Invoke(other.gameObject);
            }
        }
    }
}
