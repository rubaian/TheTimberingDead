using UnityEngine;

public class ZombieEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private float wallCheckDistance = 0.5f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRangeWidth = 1.5f;
    [SerializeField] private float attackRangeHeight = 0.8f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackDelay = 0.3f;

    [Header("References")]
    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private float leftBoundary;
    private float rightBoundary;
    private bool movingRight = true;
    private float cooldownTimer;
    private bool isAttacking;
    private bool hasAttacked;
    private Animator anim;
    private Rigidbody2D rb;
    private Health playerHealth;

    private float maxHealth = 2f;
    private float currentHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        leftBoundary = transform.position.x - patrolDistance;
        rightBoundary = transform.position.x + patrolDistance;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isAttacking) Patrol();
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= attackCooldown && !hasAttacked)
            StartAttack();
    }

    private void StartAttack()
    {
        cooldownTimer = 0;
        hasAttacked = true;
        isAttacking = true;
        anim.SetTrigger("attack");
        rb.velocity = Vector2.zero;
        Invoke(nameof(ApplyDamage), attackDelay);
    }

    private void ApplyDamage()
{
    if (PlayerInSight() && playerHealth != null && !playerHealth.IsInvincible)
    {
        Debug.Log("Zombie attacking player!"); // Debug message
        playerHealth.TakeDamage(damage);
    }
}

    private void Patrol()
    {
        if (isAttacking) return;

        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(
            attackCollider.bounds.center, 
            dir, 
            wallCheckDistance, 
            obstacleLayer
        );

        if (hit.collider != null)
        {
            Flip();
            return;
        }

        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
        anim.SetBool("moving", true);

        if ((movingRight && transform.position.x >= rightBoundary) ||
            (!movingRight && transform.position.x <= leftBoundary))
            Flip();
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool PlayerInSight()
    {
        if (attackCollider == null) return false;

        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.BoxCast(
            attackCollider.bounds.center,
            new Vector2(attackRangeWidth, attackRangeHeight),
            0f,
            dir,
            0f,
            playerLayer
        );

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        hasAttacked = false;
        cooldownTimer = 0;

        Patrol();
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, 0);
    }

    private void OnDrawGizmos()
    {
        if (attackCollider == null) return;

        Gizmos.color = Color.red;
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        Vector3 center = attackCollider.bounds.center + (Vector3)(dir * attackRangeWidth * 0.5f);
        Gizmos.DrawWireCube(center, new Vector2(attackRangeWidth, attackRangeHeight));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Zombie took damage: {damage}. Current health: {currentHealth}"); // Debug message

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!"); // Debug message
        gameObject.SetActive(false);
    }
}