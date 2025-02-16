using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start the game
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Replace "Level1" with the name of your first level scene
    }

    // Open settings menu
    public void OpenSettings()
    {
        // Add code to open the settings menu
        Debug.Log("Settings menu opened.");
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit(); // Quits the game (only works in build)

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exits play mode in the editor
        #endif
    }
}