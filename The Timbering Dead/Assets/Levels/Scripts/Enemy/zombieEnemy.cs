using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Health Parameters")]
    [SerializeField] private int maxHealth = 100; // Max health of the enemy
    private int currentHealth; // Current health of the enemy

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 2f; // Duration of the fade out effect

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;
    private SpriteRenderer rend; // Replacing Renderer with SpriteRenderer
    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Set health when the game starts
    }

    private void Update()
    {
        if (isDead) return; // Do nothing if the enemy is dead

        cooldownTimer += Time.deltaTime;

        // Attack only when the player is in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack");
                DamagePlayer(); // Call this to apply damage when attacking
            }
        }
        else
        {
            // Return to idle when not attacking
            anim.SetTrigger("Idle");
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            Vector2.left,
            0,
            playerLayer
        );

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }

    private void DamagePlayer()
    {
        if (PlayerInSight() && playerHealth != null)
            playerHealth.TakeDamage(damage);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Do not take damage after death

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die"); // Play death animation
        boxCollider.enabled = false; // Disable the collider so it no longer interacts with the world

        StartCoroutine(FadeOutAndDestroy());
    }

    // Coroutine to fade the enemy out and destroy the gameObject
    IEnumerator FadeOutAndDestroy()
    {
        Color color = rend.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        // Gradually decrease the alpha value to make the enemy fade out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            rend.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Destroy(gameObject); // Destroy the enemy after fading out
    }
}
