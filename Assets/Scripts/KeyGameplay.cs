using TMPro;
using UnityEngine;

public class KeyGameplay : MonoBehaviour
{
    public string sequence;

    public GameObject keyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //For each letter in sequence, create key object
        foreach (char key in sequence)
        {
            GameObject keyObject = Instantiate(keyPrefab);
            keyObject.transform.parent = this.transform;
            keyObject.GetComponentInChildren<TMP_Text>().text = key.ToString();
        }
        
        //After all objects are created, start timer
        Invoke("HideKeys", 3);
    }

    void HideKeys()
    {
        foreach (Transform key in this.transform)
        {
            Destroy(key.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
