using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("ConfigScene");
    }

    public void LoadGame()
    {
        IO.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
