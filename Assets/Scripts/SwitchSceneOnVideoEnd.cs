using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SwitchSceneOnVideoEnd : MonoBehaviour
{
    public int sceneToLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += OnLoopPointReached;
        vp.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnLoopPointReached(VideoPlayer vp)
    {
        vp.Stop();
        SceneManager.LoadScene(sceneToLoad);
    }
}
