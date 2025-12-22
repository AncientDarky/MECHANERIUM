using UnityEngine;
using Mechaerium;
using TMPro;
using System;
public class ModuleDisplayer : MonoBehaviour
{
    public Storage MechaStorageInstance;

    [SerializeField] internal TextMeshProUGUI[] ValueDisplayers;
    [SerializeField] internal TextMeshProUGUI[] UpgradedValuesDisplayers;

    [SerializeField] internal TextMeshProUGUI[] StandardDisplayers;

    public Action OnValueChanged;

    private void Awake()
    {
        MechaStorageInstance = Mecha.STORERAGE;
    }
    public void Upgrade()
    {

        FindAnyObjectByType<ArmourModule>().IncreaseLevel();
        OnValueChanged?.Invoke();

    }
}
