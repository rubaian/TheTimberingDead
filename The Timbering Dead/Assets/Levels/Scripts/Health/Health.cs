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
    [SerializeField] private SpriteRenderer sprite; // سيتم تعيينه تلقائيًا إذا كان null

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

        // إذا لم يتم تعيين sprite يدويًا، حاول تعيينه تلقائيًا
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
            if (sprite == null)
            {
                Debug.LogError("SpriteRenderer component is missing on this GameObject.");
            }
        }
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
            if (sprite != null) // تأكد من أن sprite موجود قبل استخدامه
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
        if (sprite != null) sprite.color = Color.white;
        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 1;
        }

        GetComponent<Collider2D>().enabled = true;
        anim.ResetTrigger("die");
    }
}