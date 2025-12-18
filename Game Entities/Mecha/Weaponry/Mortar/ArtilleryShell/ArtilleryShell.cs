using Mechaerium;
using UnityEngine;
using UnityEngine.VFX;

public class ArtilleryShell : MonoBehaviour
{
    SphereCollider Colli;
    Animator animator;
    [SerializeField] Damage damage;
    [SerializeField] float LandingDelay;
    [SerializeField] GameObject WarningHUD;
    GameObject Ref_WarnHUD;
 
    private void Awake()
    {

        animator = GetComponent<Animator>();
        animator.enabled = false;

        Ref_WarnHUD = Instantiate(WarningHUD);
        Ref_WarnHUD.transform.SetParent(FindAnyObjectByType<HUDs>().WorldHUD.transform);
        Ref_WarnHUD.transform.position = new Vector3(this.transform.position.x,FindAnyObjectByType<HUDs>().WorldHUD.transform.position.y,this.transform.position.z);
    }
    public void Init(float LifeTime)
    {
        animator.enabled = true;

        animator.SetFloat("Delay",1 * LifeTime);
        Invoke("Exploded",1 * LifeTime);
    }
    void Exploded()
    {
        this.GetComponentInChildren<VisualEffect>().Play();
        Debug.Log("enemy Hit");
        Destroy(Ref_WarnHUD);
    }
    private void OnTriggerEnter(Collider other)
    {
        this.GetComponentInChildren<VisualEffect>().Play();

        if (other.gameObject.tag == "Enemy")
        {

        }
    
    }
    void Remove()
    {
       Destroy(this.gameObject); 
    }
   
}
