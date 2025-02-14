using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    
    [Header("Health Parameters")]
    [SerializeField] private int maxHealth = 100; // أقصى صحة للعدو
    private int currentHealth; // صحة العدو الحالية

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 2f; // مدة التلاشي
    
    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;
    private SpriteRenderer rend; // استبدال Renderer بـ SpriteRenderer
    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // ضبط الصحة عند بداية اللعبة
    }

    private void Update()
    {
        if (isDead) return; // لا تفعل شيئًا إذا مات العدو

        cooldownTimer += Time.deltaTime;

        // الهجوم فقط عندما يكون اللاعب في النطاق
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack");
            }
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
        if (isDead) return; // لا تأخذ ضرر بعد الموت

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("die"); // تشغيل أنيميشن الموت
        boxCollider.enabled = false; // تعطيل الكوليدر حتى لا يتفاعل مع العالم

        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        Color color = rend.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            rend.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Destroy(gameObject); // حذف العدو بعد الاختفاء
    }
}
