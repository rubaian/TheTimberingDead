using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    private void Awake()
    {
        gameOverScreen.SetActive(false); // Ensure the game over screen is hidden at start
    }

    // Activate the game over screen
    public void GameOver()
    {
        Debug.Log("Game Over screen activated!"); // Confirm game over screen activation
        gameOverScreen.SetActive(true);
    }

    // Restart the current level
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Return to the main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Quit the game
    public void Quit()
    {
        Application.Quit(); // Quits the game (only works in build)

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exits play mode in the editor
        #endif
    }
}