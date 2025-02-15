using System.Collections;
using UnityEngine;

public class zombieEnemy : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 2f;  // Speed of movement
    [SerializeField] private float patrolDistance = 5f; // Distance to move left and right
    private float leftBoundary;
    private float rightBoundary;
    private bool movingRight = true;
    
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int damage = 10;
    
    [Header("Components")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isDead = false;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; // Prevent falling
            rb.freezeRotation = true; // Prevent rotation
        }

        // Set patrol boundaries based on start position
        leftBoundary = transform.position.x - patrolDistance;
        rightBoundary = transform.position.x + patrolDistance;
    }

    private void Update()
    {
        if (isDead) return; // Stop movement if dead

        cooldownTimer += Time.deltaTime;

        Patrol(); // Move left and right

        if (PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            anim.SetTrigger("attack"); // Trigger attack animation
            Debug.Log("Attack triggered!"); // Add this line for debugging
            DamagePlayer();
        }
    }

    private void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            anim.SetBool("moving", true); // Set moving animation

            if (transform.position.x >= rightBoundary)
            {
                Flip();
                movingRight = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            anim.SetBool("moving", true); // Set moving animation

            if (transform.position.x <= leftBoundary)
            {
                Flip();
                movingRight = true;
            }
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private bool PlayerInSight()
    {
        Vector2 rayOrigin = boxCollider.bounds.center;
        Vector2 rayDirection = movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, attackRange, playerLayer);

        Debug.DrawRay(rayOrigin, rayDirection * attackRange, Color.red); // Draw a red line to visualize the attack range

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        Vector2 rayOrigin = boxCollider.bounds.center;
        Vector2 rayDirection = movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, attackRange, playerLayer);

        if (hit.collider != null)
        {
            Health playerHealth = hit.transform.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit!"); // Add this line for debugging
            }
        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("die"); // Trigger die animation
        rb.velocity = Vector2.zero; // Stop movement
        boxCollider.enabled = false;
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        Color color = rend.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;
        float fadeDuration = 2f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            rend.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}