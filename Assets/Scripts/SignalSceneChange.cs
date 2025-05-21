using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalSceneChange : MonoBehaviour
{
    public int sceneId;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneId);
    }
}
