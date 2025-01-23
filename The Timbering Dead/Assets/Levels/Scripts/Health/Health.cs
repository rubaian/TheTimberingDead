
using UnityEngine;
using UnityEngine.Scripting;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;

    private void Awake()
    {
        currentHealth=startingHealth;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage,0,startingHealth);
        
        if(currentHealth>0)
        {
            //player hurt
        }
        else
        {
            //player dead
        }
    }
}
