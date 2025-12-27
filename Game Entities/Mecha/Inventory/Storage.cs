using Mechaerium;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    Dictionary<MaterialTypes, float> Inventory = new Dictionary<MaterialTypes, float>();
    Cost[] TotalConsump; 

    public Storage()
    {
        Inventory.Add(MaterialTypes.Iron,0);
        Inventory.Add(MaterialTypes.Copper, 0);
        Inventory.Add(MaterialTypes.Oil, 0);
        Inventory.Add(MaterialTypes.Sulfur, 0);
        Inventory.Add(MaterialTypes.WModule, 0);
        Inventory.Add(MaterialTypes.UModule, 0);
        TotalConsump = new Cost[Inventory.Count - 1];
        for (int a = 0; a < TotalConsump.Length; a++)
        {
            foreach(MaterialTypes key in Inventory.Keys)
            {
                TotalConsump[a] = new Cost();
                TotalConsump[a].Material = key;
                TotalConsump[a].Value = 0;
            }
        }
    }
    public bool HasEnoughMaterial(MaterialTypes Mat, float Amount)
    {

        bool A = Inventory[Mat] <= Amount;

        if (A)
        {

            ConsumingMats(Mat,Amount);

        }

        return A;
    }
    public void ConsumingMats(MaterialTypes Mat, float Amount)
    {
        Inventory[Mat] = Mathf.Clamp(Inventory[Mat] - Amount, 0,Mathf.Infinity);
    }
    public void IncreasingAmmunitionConsumption(MaterialTypes Mat,float Amt,bool Increases)
    {
        if (Increases)
        {
            for(int i = 0; i < TotalConsump.Length;i++)
            {
                if (TotalConsump[i].Material == Mat)
                {
                    TotalConsump[i].Value += Amt;
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
                    TotalConsump[i].Value -= Amt;
                    break;
                }
            }
        }
    }
    public void HarvestingMats(MaterialTypes Mat, float Amount)
    {
        Inventory[Mat] = Mathf.Clamp(Inventory[Mat] + Amount, 0, Mathf.Infinity);
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
}
