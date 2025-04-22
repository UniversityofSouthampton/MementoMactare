using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KeyGameplay : MonoBehaviour
{
    public static KeyGameplay instance;
    [Header("Fields")] 
    private Attack currentAttack;
    private string currentSequence;
    public List<Attack> attackSequence;
    private int currentEnemyId = -1;
    private int currentAttackId = -1;
    private int currentKeyId = 0;
    [FormerlySerializedAs("canvas")] public GameObject container;
    public GameObject keyContainerPrefab;
    private GameObject currentKeyContainer;
    public GameObject keyPrefab;
    [SerializeField] int attackIndex = 0; //used to determine which attack to perform, if a sequence has several attacks. i.e. if this is 0, it performs the first attack. if its 1, it performs the second.
    [SerializeField] private List<AttackType> learnedAttacks;

    [Header("Configuration")]
    public float memorisationTime;

    [Header("Debug")]
    public bool recallPhase;

    [Header("UI")] 
    public List<Sprite> attackSprites;
    public List<Sprite> attackLearnedSprites;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (learnedAttacks == null)
            learnedAttacks = new List<AttackType>();
    }

    void StartSequence(int attackId)
    {
        ClearKeys();
        StartSpecificSequence(attackSequence[attackId]);
    }
    
    public void StartSpecificSequence(Attack attackSequence)
    {
        ClearKeys();
        currentAttack = attackSequence;
        foreach (AttackType attack in currentAttack.attacks)
        {
            currentSequence = "";
            currentSequence += GetKeysForAttack(attack);
            currentKeyContainer = Instantiate(keyContainerPrefab, container.transform);

            
            if (!learnedAttacks.Contains(attack))
            {
                Debug.Log(attackSprites);
                currentKeyContainer.transform.GetChild(0).GetComponent<Image>().sprite = attackSprites[(int)attack];
                currentKeyContainer.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                foreach (char key in GetKeysForAttack(attack))
                {
                    SpawnKeyObject(key.ToString());
                }
            }
            else
            {
                currentKeyContainer.transform.GetChild(0).GetComponent<Image>().sprite = attackLearnedSprites[(int)attack];
                currentKeyContainer.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(35, 35);
            }
        }
        
        //After all objects are created, start timer
        Invoke("StartRecall", memorisationTime);
    }

    void SpawnKeyObject(string key)
    {
        //Create clone of key "template" aka prefab
        GameObject keyObject = Instantiate(keyPrefab);
        
        //Set the parent of the key object to be the KeyContainer
        keyObject.transform.parent = currentKeyContainer.transform;
        
        //Set the text of the keyobject to our specific text
        keyObject.GetComponentInChildren<TMP_Text>().text = key;
    }

    
    //Called after 3 seconds
    void StartRecall()
    {
        ClearKeys();
        currentKeyContainer = container.transform.GetChild(0).gameObject;
        currentKeyContainer.transform.GetChild(0).GetComponent<Image>().sprite = attackLearnedSprites[(int)currentAttack.attacks[0]];
        currentKeyContainer.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(35, 35);
        currentSequence = GetKeysForAttack(currentAttack.attacks[0]);

        //Set "RecallPhase" flag to true
        //Set the current key id to 0 (we start counting from 0)
        //For example if our currentSequence was "ABCDE", 0 would be A, 1 would be B, etc.
        recallPhase = true;
        currentAttackId = 0;
        currentKeyId = 0;
    }

    void ClearKeys(bool clearAll = false)
    {
        foreach (Transform keyContainer in container.transform)
        {
            //For each child of the KeyContainer object
            foreach (Transform child in keyContainer)
            {
                if (!child.CompareTag("AttackIcon") || clearAll)
                {
                    Destroy(child.gameObject);
                }
            }

            if (clearAll)
            {
                Destroy(keyContainer.gameObject);
            }
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
                        attackIndex += 1;
                    }
                    if (currentKeyId == currentSequence.Length)
                    {
                        Debug.Log("Completed Attack");
                        if (!learnedAttacks.Contains(currentAttack.attacks[currentAttackId]))
                        {
                            learnedAttacks.Add(currentAttack.attacks[currentAttackId]);
                        }
                        foreach (Transform key in container.transform.GetChild(currentAttackId).transform)
                        {
                            key.GetComponent<Image>().color = Color.green;
                        }
                        currentAttackId++;

                        if (currentAttackId != currentAttack.attacks.Length)
                        {
                            currentKeyId = 0;
                            currentKeyContainer = container.transform.GetChild(currentAttackId).gameObject;
                            currentKeyContainer.transform.GetChild(0).GetComponent<Image>().sprite = attackLearnedSprites[(int)currentAttack.attacks[currentAttackId]];
                            currentKeyContainer.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(35, 35);
                            currentSequence = GetKeysForAttack(currentAttack.attacks[currentAttackId]);
                            //For each child of the KeyContainer object
                        }
                        else
                        {
                            PlayerManager.instance.enemyAnimator.SetBool("Defeated",true);
                            recallPhase = false;
                            PlayerManager.instance.playerLocomotionManager.canMove = true; //temporary i think? -JR
                            attackIndex = 0;

                            //Invoke("ClearKeys(clearAll:true)", 1);
                            ClearKeys(clearAll: true);
                        }
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
        string attackToPerform = currentAttack.attacks[attackIndex].ToString();
        PlayerManager.instance.playerAnimatorManager.PlayAttackAnimation(attackToPerform);
     
    }
    private void PerformEnemyReaction()
    {
        string attackToPerform = currentAttack.reactions[attackIndex].ToString();
        PlayerManager.instance.enemyAnimator.Play(attackToPerform);
  
    }
    public void NextSequence()
    {
        ClearKeys();
        currentEnemyId = currentEnemyId + 1;
        if (currentEnemyId < attackSequence.Count)
        {
            StartSequence(currentEnemyId);
        }
        else
        {
            Debug.Log("ALL SEQUENCES COMPLETE");
            PlayerManager.instance.playerLocomotionManager.canMove = true;
        }
    }
    
    public string GetKeysForAttack(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Punch:
                return "ILM";
            case AttackType.Kick:
                return "YHU";
            case AttackType.Chop:
                return "XQZ";
            case AttackType.Elbow:
                return "KOP";
            case AttackType.Knee:
                return "TRE";
            case AttackType.Palm:
                return "NGF";
            default:
                return "";
        }
    }
}
