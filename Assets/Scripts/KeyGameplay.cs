using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyGameplay : MonoBehaviour
{
    [Header("Fields")]
    private string currentSequence;
    public List<Attack> attackSequence;
    private int currentAttackId = -1;
    public int currentKeyId = 0;
    public GameObject keyContainer;
    public GameObject keyPrefab;
    [SerializeField] int attackIndex = 0; //used to determine which attack to perform, if a sequence has several attacks. i.e. if this is 0, it performs the first attack. if its 1, it performs the second.

    [Header("Configuration")]
    public float memorisationTime;

    [Header("Debug")]
    public bool recallPhase;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void StartSequence(int attackId)
    {
        currentSequence = attackSequence[attackId].sequence;
        
        //For each letter in currentSequence, create key object
        foreach (char key in currentSequence)
        {
            //Spawns key with specific text 
            SpawnKeyObject(key.ToString());
        }
        
        //After all objects are created, start timer
        Invoke("StartRecall", memorisationTime);
    }

    void SpawnKeyObject(string key)
    {
        //Create clone of key "template" aka prefab
        GameObject keyObject = Instantiate(keyPrefab);
        
        //Set the parent of the key object to be the KeyContainer
        keyObject.transform.parent = keyContainer.transform;
        
        //Set the text of the keyobject to our specific text
        keyObject.GetComponentInChildren<TMP_Text>().text = key;
    }

    
    //Called after 3 seconds
    void StartRecall()
    {
        ClearKeys();

        //Set "RecallPhase" flag to true
        //Set the current key id to 0 (we start counting from 0)
        //For example if our currentSequence was "ABCDE", 0 would be A, 1 would be B, etc.
        recallPhase = true;
        currentKeyId = 0;
    }

    void ClearKeys()
    {
        //For each child of the KeyContainer object
        foreach (Transform key in keyContainer.transform)
        {
            //Destroy the child (delete the key object)
            Destroy(key.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleSequenceGameplay();
    }
    
    private void HandleSequenceGameplay()
    {
        //Every frame
        //Check if recallPhase flag is true
        if (recallPhase == true)
        {
            //If the player is pressing a key
            if (Input.inputString != "")
            {
                //If the key is the one that we want
                if (Input.inputString == currentSequence[currentKeyId].ToString().ToLower())
                {
                    Debug.Log("CORRECT");
                    //Spawn key object with correct text
                    SpawnKeyObject(currentSequence[currentKeyId].ToString());
                    //Add one to the currentKeyId
                    currentKeyId = currentKeyId + 1;
                    if (currentKeyId % 3 == 0)
                    {
                        PerformAttackAnimation();
                        PerformEnemyReaction();
                    }
                    if (currentKeyId == currentSequence.Length)
                    {
                        Debug.Log("Completed Sequence");
                        recallPhase = false;
                        PlayerManager.instance.playerLocomotionManager.canMove = true; //temporary i think? -JR
                        attackIndex = 0;

                        //Set to green
                        //For each child of the KeyContainer object
                        foreach (Transform key in keyContainer.transform)
                        {
                            key.GetComponent<Image>().color = Color.green;
                        }

                        Invoke("ClearKeys", 1);
                    }
                }
                //If the key is not the one that we want
                else
                {
                    Debug.Log("INCORRECT");
                }
            }
        }
    }

    private void PerformAttackAnimation()
    {
        string attackToPerform = attackSequence[currentAttackId].attacks[attackIndex].ToString();
        PlayerManager.instance.playerAnimatorManager.PlayAttackAnimation(attackToPerform);
        attackIndex += 1;
    }
    private void PerformEnemyReaction()
    {
        string attackToPerform = attackSequence[currentAttackId].reactions[attackIndex].ToString();
        PlayerManager.instance.enemyAnimator.Play(attackToPerform);
        attackIndex += 1;
    }
    public void NextSequence()
    {
        ClearKeys();
        currentAttackId = currentAttackId + 1;
        if (currentAttackId < attackSequence.Count)
        {
            StartSequence(currentAttackId);
        }
        else
        {
            Debug.Log("ALL SEQUENCES COMPLETE");
            PlayerManager.instance.playerLocomotionManager.canMove = true;
        }
    }
}
