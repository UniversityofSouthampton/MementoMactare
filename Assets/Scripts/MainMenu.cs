using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Called by Play Button
    public void Play()
    {
        //Loads scene with id 1 (the one specified in the build profile)
        SceneManager.LoadSceneAsync(1);
    }
    
    //Called by Exit Button
    public void Exit()
    {
        Application.Quit();
    }
}
