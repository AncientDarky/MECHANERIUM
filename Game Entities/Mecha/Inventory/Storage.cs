using Mechaerium;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Storage
{
    public Dictionary<MaterialTypes, float> Inventory = new Dictionary<MaterialTypes, float>();
    public Cost[] TotalConsump;
    public float TOTALMATS;
    public float IronPercentile {
      get {
            return (Inventory[MaterialTypes.Iron] * 100) / TOTALMATS; 
          }
    }
    public float CopperPercentile
    {
        get
        {
            return (Inventory[MaterialTypes.Copper] * 100) / TOTALMATS;
        }
    }
    public float OilPercentile
    {
        get
        {
            return (Inventory[MaterialTypes.Oil] * 100) / TOTALMATS;
        }
    }
    public float SulfurPercentile
    {
        get
        {
            return (Inventory[MaterialTypes.Sulfur] * 100) / TOTALMATS;
        }
    }

    public Storage()
    {
        Inventory.Add(MaterialTypes.Iron,1000);
        Inventory.Add(MaterialTypes.Copper, 250);
        Inventory.Add(MaterialTypes.Oil, 50);
        Inventory.Add(MaterialTypes.Sulfur, 250);
        Inventory.Add(MaterialTypes.WModule, 10);
        Inventory.Add(MaterialTypes.UModule, 10);
        TotalConsump = new Cost[Inventory.Count - 1]; 
        float Tots = 0;
        foreach (MaterialTypes mat in Inventory.Keys)
        {
            Tots += Inventory[mat];

        }
        TOTALMATS = Tots;

        TotalConsump[0] = new Cost();
        TotalConsump[1] = new Cost();
        TotalConsump[2] = new Cost();
        TotalConsump[3] = new Cost();

        TotalConsump[0].Material = MaterialTypes.Iron;
        TotalConsump[1].Material = MaterialTypes.Copper;
        TotalConsump[2].Material = MaterialTypes.Sulfur;
        TotalConsump[3].Material = MaterialTypes.Oil;

        TotalConsump[0].Value = 0;
        TotalConsump[1].Value = 0;
        TotalConsump[2].Value = 0;
        TotalConsump[3].Value = 0;

    }
    public bool CheckForMats(MaterialTypes Mat, float Amount)
    {

        bool A = Inventory[Mat] >= Amount;

        return A;
    }
    public bool HasEnoughMaterial(MaterialTypes Mat, float Amount)
    {

        bool A = Inventory[Mat] >= Amount;

        if (A)
        {

            ConsumingMats(Mat,Amount);

        }

        return A;
    }
    public void ConsumingMats(MaterialTypes Mat, float Amount)
    {
        Inventory[Mat] = Mathf.Clamp(Inventory[Mat] - Amount, 0,Mathf.Infinity);
        float Tots = 0;
        foreach(MaterialTypes mat in Inventory.Keys)
        {
            Tots += Inventory[mat];

        }
        TOTALMATS = Tots;
    }
    public void ConsumingAmmoMats()
    {
        Debug.Log("Consumedammot");
        for (int i = 0; i < TotalConsump.Length; i++)
        {
            if (Inventory.ContainsKey(TotalConsump[i].Material))
            {
                Debug.Log("Consumedammot" + TotalConsump[i].Material + "amount of " + TotalConsump[i].Value);
                Inventory[TotalConsump[i].Material] = Mathf.Clamp( Inventory[TotalConsump[i].Material] - TotalConsump[i].Value * Time.deltaTime,0,Mathf.Infinity);

            }
        }
    }
    public void IncreasingAmmunitionConsumption(MaterialTypes Mat,float Amt,bool Increases)
    {
        if (Increases)
        {
            for(int i = 0; i < TotalConsump.Length;i++)
            {
                if (TotalConsump[i].Material == Mat)
                {
                    TotalConsump[i].Value = Mathf.Clamp(TotalConsump[i].Value + Amt, 0, Mathf.Infinity);
                    break;
                }
            }
        }
        else
        {

            for (int i = 0; i < TotalConsump.Length; i++)
            {
                if (TotalConsump[i].Material == Mat)
                {
                    TotalConsump[i].Value = Mathf.Clamp(TotalConsump[i].Value - Amt,0,Mathf.Infinity);
                    break;
                }
            }
        }
    }
    public void HarvestingMats(MaterialTypes Mat, float Amount)
    {
        Inventory[Mat] = Mathf.Clamp(Inventory[Mat] + Amount, 0, Mathf.Infinity);
        float Tots = 0;
        foreach (MaterialTypes mat in Inventory.Keys)
        {
            Tots += Inventory[mat];

        }
        TOTALMATS = Tots;

    }
    public float[] GetInventoryValues()
    {
        float[] Values = new float[Inventory.Count - 2];
        Values[0] = Inventory[MaterialTypes.Iron];
        Values[1] = Inventory[MaterialTypes.Copper];
        Values[2] = Inventory[MaterialTypes.Oil];
        Values[3] = Inventory[MaterialTypes.Sulfur];

        return Values;
    }
    public float[] GetConsumptionValues()
    {
        float[] ConsumptionValues = new float[Inventory.Count - 2];
        for(int i = 0; i < 4;i++)
        {
            ConsumptionValues[i] = TotalConsump[i].Value;
        }
        return ConsumptionValues;
    }
    public MaterialTypes GetLowestMaterial()
    {
        Dictionary<float, MaterialTypes> SortBaseOnValue = new Dictionary<float, MaterialTypes>();

        List<float> InventoryVlauesUnsorted = new List<float>();

        foreach (MaterialTypes mat in Inventory.Keys)
        {

            if (mat == MaterialTypes.WModule || mat == MaterialTypes.UModule)
            { continue; }
            InventoryVlauesUnsorted.Add(Inventory[mat]);
        }

        InventoryVlauesUnsorted.Sort();

        MaterialTypes Lowest = MaterialTypes.None;

        foreach (MaterialTypes m in Inventory.Keys)
        {

            if (m == MaterialTypes.WModule || m == MaterialTypes.UModule)
            { continue; }

            if (Inventory[m] == InventoryVlauesUnsorted[0])
            {
                Lowest = m;
                break;
            }

        }
        return Lowest;
    }
    public MaterialTypes GetHighestMaterial()
    {
        Dictionary<float, MaterialTypes> SortBaseOnValue = new Dictionary<float, MaterialTypes>();
        

        List<float> InventoryVlauesUnsorted = new List<float>();

        foreach (MaterialTypes mat in Inventory.Keys)
        {

            if (mat == MaterialTypes.WModule || mat == MaterialTypes.UModule)
            { continue; }

            InventoryVlauesUnsorted.Add(Inventory[mat]);
        }

        InventoryVlauesUnsorted.Sort();
        MaterialTypes Highest = MaterialTypes.None;

        foreach (MaterialTypes m in Inventory.Keys)
        {
            if(m == MaterialTypes.WModule || m == MaterialTypes.UModule) { continue; }
            if (Inventory[m] == InventoryVlauesUnsorted[InventoryVlauesUnsorted.Count - 1])
            {
                Highest = m;
                break;
            }
        }

        return Highest;
    }
}
