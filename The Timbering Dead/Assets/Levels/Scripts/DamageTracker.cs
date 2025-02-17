using UnityEngine;

public class DamageTracker : MonoBehaviour
{
    private int hitCount = 0; // Counter for hits before disappearing
    private const int maxHits = 2; // Number of hits before the zombie disappears

    public void TakeDamage(int damage)
    {
        hitCount++; // Increment hit count
        if (hitCount >= maxHits)
        {
            // Hide the zombie after receiving the maximum number of hits
            gameObject.SetActive(false);
            Debug.Log(gameObject.name + " has been defeated!");
        }
    }
}