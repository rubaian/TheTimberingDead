using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Time between consecutive attacks
    [SerializeField] private float attackRange = 0.5f; // Range of the attack (how far it reaches)
    [SerializeField] private LayerMask enemyLayer; // The layer the enemies (zombies) belong to

    [Header("Sounds")]
    [SerializeField] private AudioClip attackSound; // Sound for attacking

    private Animator anim;
    private Playermovement playerMovement;
    private float cooldownTimer = Mathf.Infinity; // Timer for cooldown between attacks

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Get the animator attached to the player
        playerMovement = GetComponent<Playermovement>(); // Get the player movement script
    }

    private void Update()
    {
        // If the attack button is pressed and cooldown is over and the player can attack
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.CanAttack())
            Attack(); // Perform attack

        cooldownTimer += Time.deltaTime; // Increase cooldown timer
    }

    private void Attack()
    {
        anim.SetTrigger("L Attack"); // Play the attack animation trigger
        cooldownTimer = 0; // Reset the cooldown timer
        SoundsManager.instance.PlaySound(attackSound); // Play attack sound

        // Find all enemies within the attack range based on player's position
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        Debug.Log("Enemies detected: " + enemiesToDamage.Length); // Debug to check if enemies are detected

        foreach (Collider2D enemy in enemiesToDamage)
        {
            if (enemy != null)
            {
                // Check if the enemy has a DamageTracker component
                DamageTracker damageTracker = enemy.GetComponent<DamageTracker>();
                if (damageTracker != null)
                {
                    damageTracker.TakeDamage(1); // Apply 1 damage (adjust value as needed)
                    Debug.Log("Enemy hit: " + enemy.name); // Debug to confirm damage application
                }
            }
        }
    }

    // Visualize the attack range in the editor when selected
    private void OnDrawGizmosSelected()
    {
        // Draw a wireframe circle to represent the attack range around the player
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}