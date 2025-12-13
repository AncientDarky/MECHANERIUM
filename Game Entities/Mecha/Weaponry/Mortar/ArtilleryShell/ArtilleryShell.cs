using Mechaerium;
using UnityEngine;

public class ArtilleryShell : MonoBehaviour
{
    SphereCollider Colli;
    Animator animator;
    [SerializeField] Damage damage;
    [SerializeField] float LandingDelay;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Delay",1 / LandingDelay);
        Invoke("Remove",LandingDelay + 1);

    }
    private void OnTriggerEnter(Collider other)
    {
    if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("enemy Hit");
        }
    
    }
    void Remove()
    {
       Destroy(this.gameObject); 
    }
   
}
