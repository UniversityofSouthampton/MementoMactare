using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _blackoutImage;
    
    public void Update()
    {
        if (_blackoutImage.alpha > 0)
        {
            _blackoutImage.alpha -= Time.deltaTime;
            AudioManager.instance.PlaySound("Game Over", volume: 2f);
        }
    }


    //Called by Restart Button
    public void Restart()
    {
        PlayerHealth.instance.ResetHealth();
        
        //Loads scene with id 1 (the one specified in the build profile)
        SceneManager.LoadSceneAsync(2);
    }
    
    //Called by Exit Button
    public void Exit()
    {
        Application.Quit();
    }
}
