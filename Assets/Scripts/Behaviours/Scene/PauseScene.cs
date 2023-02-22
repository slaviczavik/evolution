using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Core.Game.isPaused)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync("PauseScene");
        Core.Game.Resume();
    }

    public void SaveGame()
    {
        IO.SaveGame();
    }

    public void LoadGame()
    {
        IO.LoadGame();
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("ConfigScene", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}