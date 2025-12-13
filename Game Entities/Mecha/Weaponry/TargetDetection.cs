using Mechaerium;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    SphereCollider DetectionShape;
    float DetectionRange;
    public WeaponModule weaponModule;

    public List<GameObject> TargetsWithinRange = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        TargetsWithinRange.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if(TargetsWithinRange.Contains(other.gameObject))
        {
            TargetsWithinRange.Remove(other.gameObject);
        }

    }
}
