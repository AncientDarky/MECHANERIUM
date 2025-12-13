using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    Dictionary<MaterialTypes, float> Inventory = new Dictionary<MaterialTypes, float>();


    public Storage()
    {
        Inventory.Add(MaterialTypes.Iron,0);
        Inventory.Add(MaterialTypes.Copper, 0);
        Inventory.Add(MaterialTypes.Oil, 0);
        Inventory.Add(MaterialTypes.Sulfur, 0);
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
    public void HarvestingMats(MaterialTypes Mat, float Amount)
    {
        Inventory[Mat] = Mathf.Clamp(Inventory[Mat] + Amount, 0, Mathf.Infinity);
    }
}
