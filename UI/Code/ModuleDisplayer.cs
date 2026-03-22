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

    public Module OwnerModule;

    private void Awake()
    {
        MechaStorageInstance = FindAnyObjectByType<Mecha>().MechStorage;
    }
    public void Upgrade()
    {

        OwnerModule.IncreaseLevel();

        OnValueChanged?.Invoke();

    }
}
