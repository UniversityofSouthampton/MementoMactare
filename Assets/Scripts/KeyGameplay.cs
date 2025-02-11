using TMPro;
using UnityEngine;

public class KeyGameplay : MonoBehaviour
{
    public string sequence;
    public int currentKeyId = 0;

    public float memorisationTime;

    public GameObject keyPrefab;

    public bool recallPhase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //For each letter in sequence, create key object
        foreach (char key in sequence)
        {
            //Spawns key with specific text 
            SpawnKeyObject(key.ToString());
        }
        
        //After all objects are created, start timer
        Invoke("HideKeys", memorisationTime);
    }

    void SpawnKeyObject(string key)
    {
        //Create clone of key "template" aka prefab
        GameObject keyObject = Instantiate(keyPrefab);
        
        //Set the parent of the key object to be the KeyContainer
        //We can use this.transform as this script is on the KeyContainer
        keyObject.transform.parent = this.transform;
        
        //Set the text of the keyobject to our specific text
        keyObject.GetComponentInChildren<TMP_Text>().text = key;
    }

    
    //Called after 3 seconds
    void HideKeys()
    {
        //For each child of the KeyContainer object
        foreach (Transform key in this.transform)
        {
            //Destroy the child (delete the key object)
            Destroy(key.gameObject);
        }

        //Set "RecallPhase" flag to true
        //Set the current key id to 0 (we start counting from 0)
        //For example if our sequence was "ABCDE", 0 would be A, 1 would be B, etc.
        recallPhase = true;
        currentKeyId = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Every frame
        //Check if recallPhase flag is true
        if (recallPhase == true)
        {
            //If the player is pressing a key
            if (Input.inputString != "")
            {
                //If the key is the one that we want
                if (Input.inputString == sequence[currentKeyId].ToString().ToLower())
                {
                    Debug.Log("CORRECT");
                    //Spawn key object with correct text
                    SpawnKeyObject(sequence[currentKeyId].ToString());
                    //Add one to the currentKeyId
                    currentKeyId = currentKeyId + 1;
                }
                //If the key is not the one that we want
                else
                {
                    Debug.Log("INCORRECT");
                }
            }
        }
    }
}
