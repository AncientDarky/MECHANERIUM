using Mechaerium;
using UnityEngine;

public class CPUModuleDisplayer : ModuleDisplayer
{
    CPUModule cpuModule;
    private void Start()
    {
        cpuModule = FindAnyObjectByType<CPUModule>();
        OwnerModule = cpuModule;
        UpdatingValues();
        OnValueChanged += UpdatingValues;
    }
    private void OnDisable()
    {
        OnValueChanged -= UpdatingValues;

    }
    void UpdatingValues()
    {
        float[] CurrentVs = new float[2];
        CurrentVs = cpuModule.TransferModuleValues(cpuModule.ModuleLevel);
        float[] UpgradedVs = new float[2];
        UpgradedVs = cpuModule.TransferModuleValues(cpuModule.ModuleLevel + 1);

        StandardDisplayers[0].text = (cpuModule.ModuleLevel + 1).ToString();
        StandardDisplayers[1].text = cpuModule.UPGRADECOSTAMT.ToString();

        ValueDisplayers[0].text = CurrentVs[0].ToString();
        ValueDisplayers[1].text = CurrentVs[1].ToString();

        UpgradedValuesDisplayers[0].text = UpgradedVs[0].ToString();
        UpgradedValuesDisplayers[1].text = UpgradedVs[1].ToString();
    }
}
