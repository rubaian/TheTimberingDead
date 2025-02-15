using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Vector2 spawnPoint;  // The initial spawn position
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
        spawnPoint = transform.position;  // Save the initial position
    }

    public void Respawn()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn();  // Restore player health to full
        }

        transform.position = spawnPoint;  // Move player back to the spawn point
    }
}
