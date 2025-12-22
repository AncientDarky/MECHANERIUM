using Mechaerium;
using UnityEngine;
using UnityEngine.VFX;

public class Node : MonoBehaviour
{
    [SerializeField] int HP;
    [SerializeField] Cost MaterialGain;
    [SerializeField] VisualEffectAsset VFX;
    GameObject temp;
    Storage MechaStorageInstance;



    private void Awake()
    {
        MechaStorageInstance = Mecha.STORERAGE;
    }
    public bool HarvestedTicked(int harvestingSpeed,float YieldMultiplier)
    {
        HP = Mathf.Clamp(HP - harvestingSpeed, 0, 1000);
        if(HP <= 0)
        {
            MechaStorageInstance.HarvestingMats(MaterialGain.Material, MaterialGain.Value * YieldMultiplier);
            GameObject newObject = new GameObject("DestroyVFX");
            newObject.transform.position = this.transform.position;

            newObject.AddComponent<VisualEffect>();
            newObject.GetComponent<VisualEffect>().visualEffectAsset = VFX;
            newObject.GetComponent<VisualEffect>().Play();

            temp = newObject;
            Invoke("Despawn", newObject.GetComponent<VisualEffect>().GetFloat("LifeSpan"));
            this.gameObject.SetActive(false);

            return true;
        }
        return false;

    }
    void Despawn()
    {
        Destroy(temp);
        Destroy(gameObject);
    }
}
