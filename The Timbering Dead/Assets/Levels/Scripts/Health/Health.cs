using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SpriteRenderer sprite;

    public float currentHealth { get; private set; }
    private bool isDead;
    private bool isInvincible;
    private Animator anim;
    private Rigidbody2D rb;
    private Playermovement movement;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Playermovement>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        StartCoroutine(InvincibilityRoutine());

        if (currentHealth <= 0) Die();
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float timer = 0;

        while (timer < invincibilityTime)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(blinkInterval);
            sprite.color = Color.white;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval * 2;
        }

        isInvincible = false;
    }

    public bool IsInvincible() => isInvincible;

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("die");
        movement.enabled = false;
        
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }

        GetComponent<Collider2D>().enabled = false;
        uiManager?.GameOver();
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;
        movement.enabled = true;
        sprite.color = Color.white;
        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 1;
        }

        GetComponent<Collider2D>().enabled = true;
        anim.ResetTrigger("die");
    }
}