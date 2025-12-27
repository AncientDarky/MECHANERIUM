using Mechaerium;
using UnityEngine;

public class CoreModuleDisplayer : ModuleDisplayer
{
    CoreModule CoreM;
    private void Start()
    {
        CoreM = FindAnyObjectByType<CoreModule>();
        OwnerModule = CoreM;
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

        CurrentVs = CoreM.TransferModuleValues(CoreM.ModuleLevel);

        ValueDisplayers[0].text = CurrentVs[0].ToString();
        ValueDisplayers[1].text = CurrentVs[1].ToString();
        ValueDisplayers[2].text = CurrentVs[2].ToString();
        ValueDisplayers[3].text = CurrentVs[3].ToString();


        float[] UpgradedVs = new float[4];


        UpgradedVs = CoreM.TransferModuleValues(CoreM.ModuleLevel + 1);


        UpgradedValuesDisplayers[0].text = UpgradedVs[0].ToString();
        UpgradedValuesDisplayers[1].text = UpgradedVs[1].ToString();
        UpgradedValuesDisplayers[2].text = UpgradedVs[2].ToString();
        UpgradedValuesDisplayers[3].text = UpgradedVs[3].ToString();

        StandardDisplayers[0].text = (CoreM.ModuleLevel + 1).ToString();
        StandardDisplayers[1].text = CoreM.UPGRADECOSTAMT.ToString();
    }
}
