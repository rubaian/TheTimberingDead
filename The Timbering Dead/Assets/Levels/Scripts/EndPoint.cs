using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private bool hasTriggered = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; 

            
            PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
            if (playerCamera != null)
            {
                playerCamera.StopCamera();
            }

            SceneController.instance.NextLevel();
        }
    }
}