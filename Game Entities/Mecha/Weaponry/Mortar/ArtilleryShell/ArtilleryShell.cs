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
        animator.enabled = true;
        damage = d;
        animator.SetFloat("Delay",1 * LifeTime);
        Invoke("Exploded",1 * LifeTime);
    }
    void Exploded()
    {
        this.GetComponentInChildren<VisualEffect>().Play();
        Debug.Log("enemy Hit");
        Destroy(Ref_WarnHUD);
        WithinExplosionArea.Clear();
    }
    private void OnTriggerStay(Collider other)
    {
        this.GetComponentInChildren<VisualEffect>().Play();

        if (other.gameObject.tag == "Enemy" && WithinExplosionArea.Contains(other.gameObject.GetComponent<Robites>()) == false)
        {
            other.gameObject.GetComponent<Robites>().OnDamaged?.Invoke(damage);
            WithinExplosionArea.Add(other.gameObject.GetComponent<Robites>());
        }
    
    }
    void Remove()
    {
       Destroy(this.gameObject); 
    }
   
}
