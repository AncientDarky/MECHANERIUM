using Mechaerium;
using NUnit.Framework;
using Robitnekics;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections.Generic;
public class ArtilleryShell : MonoBehaviour
{
    SphereCollider Colli;
    Animator animator;
    [SerializeField] Damage damage;
    [SerializeField] float LandingDelay;
    [SerializeField] GameObject WarningHUD;
    GameObject Ref_WarnHUD;

    List<Robites> WithinExplosionArea = new List <Robites>();
 
    private void Awake()
    {

        animator = GetComponent<Animator>();
        animator.enabled = false;
        Ref_WarnHUD = Instantiate(WarningHUD);
        Ref_WarnHUD.transform.SetParent(FindAnyObjectByType<HUDs>().WorldHUD.transform);
        Ref_WarnHUD.transform.position = new Vector3(this.transform.position.x,FindAnyObjectByType<HUDs>().WorldHUD.transform.position.y,this.transform.position.z);
    }
    public void Init(float LifeTime,Damage d)
    {

        this.GetComponentInChildren<VisualEffect>().Play();

        animator.enabled = true;
        damage = d;
        animator.SetFloat("Delay",1 * LifeTime);
    }
    public void Exploded()
    {
        this.GetComponentInChildren<VisualEffect>().Play();
        Debug.Log("enemy Hit");
        Destroy(Ref_WarnHUD);
        foreach(Robites robiter in WithinExplosionArea)
        {
            robiter.OnDamaged?.Invoke(damage);
        }

        WithinExplosionArea.Clear();
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Enemy" && WithinExplosionArea.Contains(other.gameObject.GetComponent<Robites>()) == false)
        {
            WithinExplosionArea.Add(other.gameObject.GetComponent<Robites>());
        }
    
    }
    void Remove()
    {
       Destroy(this.gameObject); 
    }
   
}
