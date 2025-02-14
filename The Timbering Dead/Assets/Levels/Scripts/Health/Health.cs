using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;
    
    // Reference to UIManager
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (dead) return; // Prevent taking damage after death

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (dead) return; // Prevent multiple calls to Die()

        dead = true; // Mark as dead

        anim.SetTrigger("die");

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;  
            rb.angularVelocity = Vector3.zero;  
            rb.isKinematic = true;  
        }

        // Disable the movement script (replace "Playermovement" if you use a different script for player movement)
        Playermovement movementScript = GetComponent<Playermovement>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // Call GameOver method to show the game over screen
        if (uiManager != null)
        {
            uiManager.GameOver();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
    }

    internal void Respawn()
    {
        throw new NotImplementedException();
    }
}
