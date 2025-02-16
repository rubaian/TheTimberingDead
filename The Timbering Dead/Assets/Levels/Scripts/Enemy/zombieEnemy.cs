using UnityEngine;

public class ZombieEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f; // Speed of the zombie
    [SerializeField] private float patrolDistance = 3f; // Distance the zombie patrols
    [SerializeField] private float wallCheckDistance = 0.5f; // Distance to check for walls

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f; // Cooldown between attacks
    [SerializeField] private float attackRangeWidth = 1.5f; // Width of the attack range
    [SerializeField] private float attackRangeHeight = 0.8f; // Height of the attack range
    [SerializeField] private int damage = 1; // Damage dealt to the player
    [SerializeField] private float attackDelay = 0.3f; // Delay before applying damage

    [Header("References")]
    [SerializeField] private BoxCollider2D attackCollider; // Collider for attack range
    [SerializeField] private LayerMask playerLayer; // Layer for the player
    [SerializeField] private LayerMask obstacleLayer; // Layer for obstacles

    private float leftBoundary; // Left boundary of patrol area
    private float rightBoundary; // Right boundary of patrol area
    private bool movingRight = true; // Direction of movement
    private float cooldownTimer; // Timer for attack cooldown
    private bool isAttacking; // Whether the zombie is attacking
    private bool hasAttacked; // Whether the zombie has already attacked in this cycle
    private Animator anim; // Animator component
    private Rigidbody2D rb; // Rigidbody2D component
    private Health playerHealth; // Reference to the player's health

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Set patrol boundaries based on starting position
        leftBoundary = transform.position.x - patrolDistance;
        rightBoundary = transform.position.x + patrolDistance;
    }

    private void Update()
    {
        if (!isAttacking) Patrol(); // Patrol if not attacking
        cooldownTimer += Time.deltaTime; // Update cooldown timer

        // Check if player is in sight, cooldown is over, and attack hasn't started
        if (PlayerInSight() && cooldownTimer >= attackCooldown && !hasAttacked)
            StartAttack();
    }

    private void StartAttack()
    {
        cooldownTimer = 0; // Reset cooldown timer
        hasAttacked = true; // Mark attack as started
        isAttacking = true; // Set attacking state
        anim.SetTrigger("attack"); // Trigger attack animation
        rb.velocity = Vector2.zero; // Stop movement during attack
        Invoke(nameof(ApplyDamage), attackDelay); // Apply damage after delay
    }

    private void ApplyDamage()
    {
        // Apply damage if player is in sight and not invincible
        if (PlayerInSight() && playerHealth != null && !playerHealth.IsInvincible)
            playerHealth.TakeDamage(damage);
    }

    private void Patrol()
    {
        if (isAttacking) return; // Stop patrolling if attacking

        Vector2 dir = movingRight ? Vector2.right : Vector2.left; // Movement direction
        RaycastHit2D hit = Physics2D.Raycast(
            attackCollider.bounds.center, 
            dir, 
            wallCheckDistance, 
            obstacleLayer
        );

        // Flip direction if wall is detected
        if (hit.collider != null)
        {
            Flip();
            return;
        }

        // Move in the current direction
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
        anim.SetBool("moving", true);

        // Flip direction if patrol boundary is reached
        if ((movingRight && transform.position.x >= rightBoundary) ||
            (!movingRight && transform.position.x <= leftBoundary))
            Flip();
    }

    private void Flip()
    {
        movingRight = !movingRight; // Reverse movement direction
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the sprite
        transform.localScale = scale;
    }

    private bool PlayerInSight()
    {
        if (attackCollider == null) return false;

        Vector2 dir = movingRight ? Vector2.right : Vector2.left; // Attack direction
        RaycastHit2D hit = Physics2D.BoxCast(
            attackCollider.bounds.center,
            new Vector2(attackRangeWidth, attackRangeHeight), // Use custom width and height
            0f,
            dir,
            0f,
            playerLayer
        );

        // Get player health if player is detected
        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    public void OnAttackEnd()
    {
        isAttacking = false; // End attack state
        hasAttacked = false; // Reset attack flag
        cooldownTimer = 0; // Reset cooldown timer

        // Resume patrolling
        Patrol();
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, 0);
    }

    private void OnDrawGizmos()
    {
        if (attackCollider == null) return;

        // Draw attack range in the editor
        Gizmos.color = Color.red;
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        Vector3 center = attackCollider.bounds.center + (Vector3)(dir * attackRangeWidth * 0.5f);
        Gizmos.DrawWireCube(center, new Vector2(attackRangeWidth, attackRangeHeight));
    }
}