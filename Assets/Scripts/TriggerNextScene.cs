using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextScene : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneNumber != 2)
            {
                AudioManager.instance.PlaySound("Area Transition", volume: 0.5f);
            }

            SceneManager.LoadScene(currentSceneNumber + 1);
        }
    }
}
