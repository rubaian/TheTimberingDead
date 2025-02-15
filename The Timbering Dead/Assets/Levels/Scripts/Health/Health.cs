using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth = 3f;  // Default starting health
    public float currentHealth;
    private Animator anim;
    private bool isDead;

    // Reference to UIManager for Game Over screen
    [SerializeField] private UIManager uiManager;

    private Playermovement movementScript;
    private Rigidbody2D rb;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        movementScript = GetComponent<Playermovement>();  // Get movement script
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D
    }

    // Take damage and handle death
    public void TakeDamage(float damage)
{
    if (isDead) return; // Prevent taking damage after death

    // Apply damage and log the health values before and after damage
    Debug.Log("Health before damage: " + currentHealth);
    currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
    Debug.Log("Damage taken! Current health: " + currentHealth);

    if (currentHealth <= 0)
    {
        Die();  // Call Die() if health reaches 0
    }
}

    // Handle death: Disable movement, play die animation, and show Game Over
    private void Die()
    {
        if (isDead) return;  // Prevent calling Die() multiple times

        isDead = true;  // Mark as dead
        anim.SetTrigger("die");  // Play die animation

        // Disable movement
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // Call GameOver method in UIManager
        if (uiManager != null)
        {
            uiManager.GameOver();
        }

        // Stop Rigidbody movement if necessary
        if (rb != null)
        {
            rb.velocity = Vector2.zero;  
            rb.angularVelocity = 0f;  // Fix the error, angularVelocity should be a float

            rb.isKinematic = true;  // Make the Rigidbody non-interactive
        }
    }

    // Reset health when the player respawns
    public void ResetHealth()
    {
        currentHealth = startingHealth;  // Reset health to full
        isDead = false;  // Mark the player as alive
        anim.ResetTrigger("die");  // Reset die animation trigger

        // Re-enable movement after respawn
        if (movementScript != null)
        {
            movementScript.enabled = true;
        }

        // Re-enable Rigidbody if necessary
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
