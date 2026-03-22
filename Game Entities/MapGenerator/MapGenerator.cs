using UnityEngine;
using Mechaerium;
using System.Collections.Generic;
public class MapGenerator : MonoBehaviour
{
    [SerializeField] float RoamingEnemySpawnChance, CrateSpawnChance, NodeSpawnChance;
    Storage MECHASTORAGE;

    List<float> ArrayMatPercentile;
    public MaterialTypes HighestMat,LowestMat;
    void Start()
    {
        ArrayMatPercentile = new List<float>();
        MECHASTORAGE = FindAnyObjectByType<Mecha>().STORERAGE;
        ArrayMatPercentile.Add(0);
        ArrayMatPercentile.Add(0);
        ArrayMatPercentile.Add(0);
        ArrayMatPercentile.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        ArrayMatPercentile[0] = MECHASTORAGE.IronPercentile;
        ArrayMatPercentile[1] = MECHASTORAGE.CopperPercentile;
        ArrayMatPercentile[2] = MECHASTORAGE.OilPercentile;
        ArrayMatPercentile[3] = MECHASTORAGE.SulfurPercentile;
        ArrayMatPercentile.Sort();
        HighestMat = MECHASTORAGE.GetHighestMaterial();
        LowestMat = MECHASTORAGE.GetLowestMaterial();
    }
}
