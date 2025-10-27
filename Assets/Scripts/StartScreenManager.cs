using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_Selection");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); // only visible in editor
    }
}
