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
    [SerializeField] private SpriteRenderer sprite; // Automatically assigned if null

    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsInvincible { get; private set; }

    private Animator anim;
    private Rigidbody2D rb;
    private Playermovement movement;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Playermovement>();

        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
            if (sprite == null)
            {
                Debug.LogError("SpriteRenderer component is missing on this GameObject.");
            }
        }

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager not found in the scene. Ensure it is added.");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || IsInvincible) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);
        StartCoroutine(InvincibilityRoutine());

        if (CurrentHealth <= 0) Die();
    }

    private IEnumerator InvincibilityRoutine()
    {
        IsInvincible = true;
        float timer = 0;

        while (timer < invincibilityTime)
        {
            if (sprite != null)
            {
                sprite.color = new Color(1, 1, 1, 0.5f);
                yield return new WaitForSeconds(blinkInterval);
                sprite.color = Color.white;
                yield return new WaitForSeconds(blinkInterval);
                timer += blinkInterval * 2;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer is not assigned.");
                break;
            }
        }

        IsInvincible = false;
    }

    private void Die()
    {
        if (IsDead) return;

        Debug.Log("Player died!"); // Confirm player death
        IsDead = true;
        anim.SetTrigger("die");
        movement.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }

        GetComponent<Collider2D>().enabled = false;

        if (uiManager != null)
        {
            Debug.Log("Game Over function called in UIManager.");
            uiManager.GameOver(); // Call GameOver() in UIManager
        }
        else
        {
            Debug.LogError("uiManager is null. Game Over screen will not be displayed.");
        }
    }

    public void Respawn()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
        movement.enabled = true;
        if (sprite != null) sprite.color = Color.white;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 1;
        }

        GetComponent<Collider2D>().enabled = true;
        anim.ResetTrigger("die");
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
    }
}
