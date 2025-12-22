using Mechaerium;
using UnityEngine;

public class ArmourDisplayer : ModuleDisplayer
{
    ArmourModule DisplayingArmour;
    private void Awake()
    {
        DisplayingArmour = FindAnyObjectByType<ArmourModule>();
    }
    private void Start()
    {
        UpdatingValues();
        OnValueChanged += UpdatingValues;
    }
    private void OnDisable()
    {
        OnValueChanged -= UpdatingValues;

    }
    void UpdatingValues ()
    {
        float[]CurrentV = new float[4];
        CurrentV = DisplayingArmour.TransferModuleValues(DisplayingArmour.ModuleLevel);
        ValueDisplayers[0].text = CurrentV[0].ToString();
        ValueDisplayers[1].text = CurrentV[1].ToString();
        ValueDisplayers[2].text = CurrentV[2].ToString();
        ValueDisplayers[3].text = CurrentV[3].ToString();
        float[]UpgradedValues = new float[4];
        UpgradedValues = DisplayingArmour.TransferModuleValues(DisplayingArmour.ModuleLevel + 1);

        UpgradedValuesDisplayers[0].text = UpgradedValues[0].ToString();
        UpgradedValuesDisplayers[1].text = UpgradedValues[1].ToString();
        UpgradedValuesDisplayers[2].text = UpgradedValues[2].ToString();
        UpgradedValuesDisplayers[3].text = UpgradedValues[3].ToString();

        StandardDisplayers[0].text = (DisplayingArmour.ModuleLevel + 1).ToString();
        StandardDisplayers[1].text = DisplayingArmour.UPGRADECOSTAMT.ToString();
    }
}
