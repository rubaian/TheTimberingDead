
using UnityEngine;
using UnityEngine.Scripting;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth=startingHealth;
        anim=GetComponent<Animator>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage,0,startingHealth);
        
        if(currentHealth>0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                GetComponent<NewBehaviourScript>().enabled = false;
                dead=true;
            }
            

        }
    }
}
