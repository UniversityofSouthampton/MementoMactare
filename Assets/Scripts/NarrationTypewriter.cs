using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NarrationTypewriter : MonoBehaviour
{

    public float textSpeed;

    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TypewriterTextEffect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator TypewriterTextEffect()
    {
        
        text.maxVisibleCharacters = 0;

        for (int i = 0; i < text.text.Length; i++)
        {
            text.maxVisibleCharacters += 1;
            if (char.IsPunctuation(text.text[i]))
            {
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(textSpeed);
        }
        GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(3f);

        yield return null;
        SceneManager.LoadScene("Opening Area (Dialogue)");
    }
}
