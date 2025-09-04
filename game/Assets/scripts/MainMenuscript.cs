using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuscript : MonoBehaviour
{
    // Call this from your Play button
    public void PlayGame()
    {
        // Make sure your game scene is added in Build Settings
        SceneManager.LoadScene("Game");
    }

    // Call this from your Quit button
    public void QuitGame()
    {
        // Will quit the game in build; does nothing in the editor
        Application.Quit();
        Debug.Log("Quit Game"); // for testing in editor
    }
}
