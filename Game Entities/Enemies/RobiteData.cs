using UnityEngine;
using Mechaerium;

[CreateAssetMenu(fileName = "RobiteData", menuName = "Scriptable Objects/RobiteData")]
public class RobiteData : ScriptableObject
{
    [Header("General Properties")]
    [SerializeField] public int MaxHP;
    [SerializeField] public float SlowSpeed,NormalSpeed,BoostSpeed;
    [SerializeField] public float AttackRange;
    [SerializeField] public float PlayerDetectionRange;
    [SerializeField] public float Attackspeed;
    [SerializeField] public Damage BaseDamage;
    [SerializeField][Range(0f, 1.0f)] public float PhysicalResist;
    [SerializeField][Range(0f, 1.0f)] public float PiercResist;
    [SerializeField][Range(0f, 1.0f)] public float HeatResist;
    [SerializeField][Range(0f, 1.0f)] public float ExploResist;


    [Header("Chances Properties")]
    [SerializeField][Range(0f,1.0f)] float StunChanceResistance;
    [SerializeField][Range(0f, 1.0f)] float BurnChanceResistance;
    [SerializeField][Range(0f, 1.0f)] float DeflectionChance;

}
