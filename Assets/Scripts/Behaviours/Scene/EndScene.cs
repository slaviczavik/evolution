using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public void Close()
    {
        SceneManager.LoadScene("MainScene");
    }
}
