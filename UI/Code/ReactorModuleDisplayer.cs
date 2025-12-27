using Mechaerium;
using UnityEngine;

public class ReactorModuleDisplayer : ModuleDisplayer
{
    ReactorModule reactorModule;
    private void Start()
    {
        reactorModule = FindAnyObjectByType<ReactorModule>();
        OwnerModule = reactorModule;
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
        CurrentVs = reactorModule.TransferModuleValues(reactorModule.ModuleLevel);
        float[] UpgradedVs = new float[3];
        UpgradedVs = reactorModule.TransferModuleValues(reactorModule.ModuleLevel + 1);

        StandardDisplayers[0].text = (reactorModule.ModuleLevel + 1).ToString();
        StandardDisplayers[1].text = reactorModule.UPGRADECOSTAMT.ToString();

        ValueDisplayers[0].text = CurrentVs[0].ToString();
        ValueDisplayers[1].text = CurrentVs[1].ToString();
        ValueDisplayers[2].text = CurrentVs[2].ToString();

        UpgradedValuesDisplayers[0].text = UpgradedVs[0].ToString();
        UpgradedValuesDisplayers[1].text = UpgradedVs[1].ToString();
        UpgradedValuesDisplayers[2].text = UpgradedVs[2].ToString();

    }
}
