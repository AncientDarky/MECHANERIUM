using UnityEngine;
using UnityEngine.AI;
using Mechaerium;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "RobiteData", menuName = "Scriptable Objects/RobiteData")]
public class RobiteData : ScriptableObject
{
    [Header("General Properties")]
    [SerializeField] int MaxHP;
    [SerializeField] public float SlowSpeed,NormalSpeed,BoostSpeed;
    [SerializeField] public float AttackRange;
    [SerializeField] public float PlayerDetectionRange;
    [SerializeField] float Attackspeed;
    [SerializeField] Damage BaseDamage;
    [SerializeField][Range(0f, 1.0f)] float PhysicalResist;
    [SerializeField][Range(0f, 1.0f)] float PiercResist;
    [SerializeField][Range(0f, 1.0f)] float HeatResist;
    [SerializeField][Range(0f, 1.0f)] float ExploResist;


    [Header("Chances Properties")]
    [SerializeField][Range(0f,1.0f)] float StunChanceResistance;
    [SerializeField][Range(0f, 1.0f)] float BurnChanceResistance;
    [SerializeField][Range(0f, 1.0f)] float DeflectionChance;

}
