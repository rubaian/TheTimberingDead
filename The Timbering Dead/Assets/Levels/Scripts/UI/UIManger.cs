using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;  // The game over screen UI element
    [SerializeField] private AudioClip gameOverSound;    // Game over sound clip (optional)
    private object soundManager;  // Reference to the sound manager, if you want to manage sounds

    private void Awake()
    {
        gameOverScreen.SetActive(false);  // Initially hide the game over screen
    }

    #region Game Over Functions
    // Function to display the game over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);  // Activate the game over screen when the player dies
        
        // Optionally, you can play a sound for the game over (uncomment to use):
        // if (gameOverSound != null) AudioSource.PlayClipAtPoint(gameOverSound, transform.position);
    }

    // Function to restart the current level
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
    }

    // Function to load the main menu scene (assuming it's at index 0)
    public void MainMenu()
    {
        SceneManager.LoadScene(0);  // Load the main menu scene (scene index 0)
    }

    // Function to quit the game or exit play mode if in the Unity Editor
    public void Quit()
    {
        Application.Quit();  // Quits the game (works only in the build)

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Exits play mode in the Unity Editor
        #endif
    }
    #endregion
}
