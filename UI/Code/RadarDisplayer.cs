using Mechaerium;
using UnityEngine;

public class RadarDisplayer : ModuleDisplayer
{
    RadarModule RadarM;
    private void Start()
    {
        RadarM = FindAnyObjectByType<RadarModule>();
        OwnerModule = RadarM;
        UpdatingValues();
        OnValueChanged += UpdatingValues;
    }
    private void OnDisable()
    {
        OnValueChanged -= UpdatingValues;

    }
    void UpdatingValues()
    {
        float[] CurrentVs = new float[3];

        CurrentVs = RadarM.TransferModuleValues(RadarM.ModuleLevel);

        ValueDisplayers[0].text = CurrentVs[0].ToString();
        ValueDisplayers[1].text = CurrentVs[1].ToString();
        ValueDisplayers[2].text = CurrentVs[2].ToString();


        float[] UpgradedVs = new float[4];


        UpgradedVs = RadarM.TransferModuleValues(RadarM.ModuleLevel + 1);


        UpgradedValuesDisplayers[0].text = UpgradedVs[0].ToString();
        UpgradedValuesDisplayers[1].text = UpgradedVs[1].ToString();
        UpgradedValuesDisplayers[2].text = UpgradedVs[2].ToString();

        StandardDisplayers[0].text = (RadarM.ModuleLevel + 1).ToString();
        StandardDisplayers[1].text = RadarM.UPGRADECOSTAMT.ToString();
    }
}
