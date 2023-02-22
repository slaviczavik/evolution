using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorScene : MonoBehaviour
{
    public void Close()
    {
        SceneManager.UnloadSceneAsync("ErrorScene");
    }
}
