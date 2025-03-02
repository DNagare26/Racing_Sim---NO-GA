using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main menu, handling scene transitions and UI navigation.
/// </summary>
public class MainMenuManager : MonoBehaviour
{

    private void Start()
    {

    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("CarConfig");
    }
    
    public void OpenGuide()
    {
        SceneManager.LoadScene("Guide");
    }


    public void CloseGuide()
    {

        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Opens the settings panel.
    /// </summary>
    public void OpenSettings()
    {

        SceneManager.LoadScene("Settings");
    }

    /// <summary>
    /// Closes the settings panel.
    /// </summary>
    public void CloseSettings()
    {

        SceneManager.LoadScene("Menu");
    }

    public void OpenResults()
    {
        SceneManager.LoadScene("Results");
    }
    /// <summary>
    /// Exits the game (Only works in a built version).
    /// </summary>
    public void ExitGame()
    {
        
        Application.Quit();
        Debug.Log("Exiting game...");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); 
#endif
    }

}