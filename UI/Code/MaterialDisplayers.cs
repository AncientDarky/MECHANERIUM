using TMPro;
using UnityEngine;

public class MaterialDisplayers : ModuleDisplayer
{
    [SerializeField] TextMeshProUGUI[] Consumptions;


    float[] MechaStorageValues;
    float[] MechaTotalConsumptionValues;

    // 0 = Iron 1 = Copper 2= Oil 3 = Sulfur
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MechaStorageValues = new float[4];
        MechaTotalConsumptionValues = new float[4];
    }

    // Update is called once per frame
    void Update()
    {
        MechaStorageValues = MechaStorageInstance.GetInventoryValues();
        MechaTotalConsumptionValues = MechaStorageInstance.GetConsumptionValues();

        ValueDisplayers[0].text = MechaStorageValues[0].ToString();
        ValueDisplayers[1].text = MechaStorageValues[1].ToString();
        ValueDisplayers[2].text = MechaStorageValues[2].ToString();
        ValueDisplayers[3].text = MechaStorageValues[3].ToString();

        Consumptions[0].text = MechaTotalConsumptionValues[0].ToString();
        Consumptions[1].text = MechaTotalConsumptionValues[1].ToString();
        Consumptions[2].text = MechaTotalConsumptionValues[2].ToString();

        Consumptions[3].text = MechaTotalConsumptionValues[3].ToString();
    }

}
