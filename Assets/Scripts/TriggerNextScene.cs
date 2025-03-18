using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextScene : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneNumber + 1);
        }
    }
}
