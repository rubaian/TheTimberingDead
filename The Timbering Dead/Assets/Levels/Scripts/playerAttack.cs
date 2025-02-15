using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;  // Time between consecutive attacks
    [SerializeField] private Transform attackPoint;  // Attack point (where the sword hits)
    [SerializeField] private float attackRange = 0.5f;  // Range of the attack (how far it reaches)
    [SerializeField] private LayerMask enemyLayer;  // The layer the enemies (zombies) belong to

    private Animator anim;
    private Playermovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;  // Timer for cooldown between attacks

    private void Awake()
    {
        anim = GetComponent<Animator>();  // Get the animator attached to the player
        playerMovement = GetComponent<Playermovement>();  // Get the player movement script
    }

    private void Update()
    {
        // If the attack button is pressed and cooldown is over and the player can attack
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();  // Perform attack

        cooldownTimer += Time.deltaTime;  // Increase cooldown timer over time
    }

    private void Attack()
    {
        anim.SetTrigger("L Attack");  // Play the attack animation trigger
        cooldownTimer = 0;  // Reset the cooldown timer

        // Find all enemies within the attack range
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        Debug.Log("Enemies detected: " + enemiesToDamage.Length); // Debug to check if enemies are detected

        foreach (Collider2D enemy in enemiesToDamage)
        {
            if (enemy != null)
            {
                // Apply damage to the enemy (zombie)
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(1);  // Apply 1 damage (adjust value as needed)
                    Debug.Log("Enemy hit: " + enemy.name); // Debug to confirm damage application
                }
            }
        }
    }

    // Visualize the attack range in the editor when selected
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Draw a wireframe circle to represent the attack range
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}