using UnityEngine;

public class ZombieEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 1f; // Time between each attack
    [SerializeField] private float range = 1f; // Attack range
    [SerializeField] private int damage = 1; // Damage dealt by the zombie

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider; // Zombie's collider
    [SerializeField] private float colliderDistance; // Distance to extend the BoxCast range

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer; // Layer for the player

    private float cooldownTimer = Mathf.Infinity; // Timer to track the cooldown between attacks
    private bool hasAttacked = false; // Flag to track if the zombie has already attacked
    private bool isAttacking = false; // Flag to track if the zombie is in attack animation

    // References
    private Animator anim; // Animator for the zombie
    private Health playerHealth; // Reference to the player's health

    private void Awake()
    {
        anim = GetComponent<Animator>();

        // Ensure boxCollider is assigned in the inspector
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime; // Increment the cooldown timer

        // Check if player is in range and cooldown has passed, and is not already attacking
        if (PlayerInSight() && cooldownTimer >= attackCooldown && !hasAttacked)
        {
            cooldownTimer = 0; // Reset cooldown timer
            hasAttacked = true; // Set the flag to prevent further attacks until cooldown
            anim.SetTrigger("attack"); // Trigger attack animation
            isAttacking = true; // Mark that the zombie is attacking
        }
        else if (cooldownTimer >= attackCooldown)
        {
            hasAttacked = false; // Reset the attack flag once cooldown has passed
        }
    }

    private bool PlayerInSight()
    {
        // BoxCast to detect if the player is within range
        RaycastHit2D hit = Physics2D.BoxCast(
            (Vector2)boxCollider.bounds.center + new Vector2(transform.right.x * range, 0), // Ensure this is a Vector2
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y), 
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>(); // Set the player's health
            return true; // Player is in range
        }
        return false; // No player detected
    }

    private void DamagePlayer()
    {
        if (isAttacking && playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Apply damage to the player
            Debug.Log("Damage applied to player. Current health: " + playerHealth.currentHealth); // Log damage
        }
    }

    // This method will be triggered by the attack animation event in the Animator
    public void OnAttackEnd()
    {
        isAttacking = false; // Reset the attack state once the attack animation ends
        hasAttacked = false; // Reset the flag to allow the next attack
    }

    private void OnDrawGizmos()
    {
        // Draw a wireframe box in the scene view to visualize attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)boxCollider.bounds.center + new Vector2(transform.right.x * range, 0), 
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y));
    }
}
