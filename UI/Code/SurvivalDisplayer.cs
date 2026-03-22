using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mechaerium;
public class SurvivalDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI RemainingTimeLeft,LifeSupp,Hitpoint;
    [SerializeField] Image HitPointfiller, LifeMatFiller;

    public static Action OnHitPointChanged;

    [SerializeField] Animation LifeSupportwarning;
    private void OnEnable()
    {
        OnHitPointChanged += UpdatingUI;
    }
    private void OnDisable()
    {
        OnHitPointChanged -= UpdatingUI;
    }
    private void LateUpdate()
    {
        UpdatingUI();
    }
    public void UpdatingUI()
    {
        Mecha mech = FindAnyObjectByType<Mecha>();
        CoreModule CoreM = FindAnyObjectByType<CoreModule>();

        if (mech != null)
        {
            HitPointfiller.fillAmount =  mech.RemainingHealPoints / mech.MaximumHealthPoints;
            LifeMatFiller.fillAmount = CoreM.REMAINLIFESUPP /  CoreM.MAXLIFECAP;
            LifeSupp.text = (Mathf.Round(LifeMatFiller.fillAmount * 100)).ToString() + " %";
            Hitpoint.text = (Mathf.Round(HitPointfiller.fillAmount * 100)).ToString() + " %";
            float TotalSeonds = CoreM.REMAINTIME;
            float ConvertedToMinutes = CoreM.REMAINTIME / 60f;
            float MinSecond = ConvertedToMinutes * 10;
            float Result = Mathf.Round(MinSecond);
            RemainingTimeLeft.text = " > " + (Result / 10).ToString() + " min ";
        }
        if(LifeSupportwarning)
        {
            if(LifeMatFiller.fillAmount < 0.25f)
            {
                if(LifeSupportwarning.isPlaying == false)
                {

                    LifeSupportwarning.Play();
                }
            }
            else
            {
                LifeSupportwarning.Stop();
            }
        }
    }
}
