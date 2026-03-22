using Mechaerium;
using UnityEngine;

public class HarvestorModuleDisplayer : ModuleDisplayer
{
    HarvestorModule HarvestModule;
    private void Start()
    {
        HarvestModule = FindAnyObjectByType<HarvestorModule>();
        OwnerModule = HarvestModule;
        UpdatingValues();
        OnValueChanged += UpdatingValues;
    }
    private void OnDisable()
    {
        OnValueChanged -= UpdatingValues;

    }
    void UpdatingValues()
    {
        float[] CurrentVs = new float[4];
        CurrentVs = HarvestModule.TransferModuleValues(HarvestModule.ModuleLevel);
        float[] UpgradedVs = new float[4];
        UpgradedVs = HarvestModule.TransferModuleValues(HarvestModule.ModuleLevel + 1);

        StandardDisplayers[0].text = (HarvestModule.ModuleLevel + 1).ToString();
        StandardDisplayers[1].text = HarvestModule.UPGRADECOSTAMT.ToString();

        ValueDisplayers[0].text = CurrentVs[0].ToString();
        ValueDisplayers[1].text = CurrentVs[1].ToString();
        ValueDisplayers[2].text = CurrentVs[2].ToString();
        ValueDisplayers[3].text = CurrentVs[3].ToString();

        UpgradedValuesDisplayers[0].text = UpgradedVs[0].ToString();
        UpgradedValuesDisplayers[1].text = UpgradedVs[1].ToString();
        UpgradedValuesDisplayers[2].text = UpgradedVs[2].ToString();
        UpgradedValuesDisplayers[3].text = UpgradedVs[3].ToString();
    }
}
