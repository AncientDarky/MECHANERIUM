using System;
using UnityEngine;

public class Robites : MonoBehaviour
{
    public Action<GameObject> OnDeath;
    public bool CallDeath;

    private void Start()
    {
        Invoke("Died",20f);
    }
   
    public void Died()
    {
        if(0 == 0)
        {
            OnDeath?.Invoke(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
