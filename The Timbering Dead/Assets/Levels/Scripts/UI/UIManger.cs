using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;  // The Game Over screen UI element
    [SerializeField] private AudioClip gameOverSound;    // Game Over sound clip (optional)

    private void Awake()
    {
        gameOverScreen.SetActive(false);  // Hide the Game Over screen at the start
    }

    #region Game Over Functions

    // Display the Game Over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);  // Show the Game Over screen

        // Play the Game Over sound if available
        if (gameOverSound != null)
        {
            AudioSource.PlayClipAtPoint(gameOverSound, transform.position);
        }
    }

    // Restart the current level
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Load the main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Quit the game
    public void Quit()
    {
        Application.Quit(); // Works only in a built application

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit play mode in Unity Editor
        #endif
    }

    #endregion
}
