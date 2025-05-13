using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] int attackIndex = 0; //used to determine which attack to perform, if a sequence has several attacks. i.e. if this is 0, it performs the first attack. if its 1, it performs the second.
    [SerializeField] private List<AttackType> learnedAttacks;

    [Header("Configuration")]
    public float memorisationTime;

    [Header("Debug")]
    public bool recallPhase;
    

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
        KeyUI.instance.ClearKeys();
        StartSpecificSequence(attackSequence[attackId]);
    }
    
    public void StartSpecificSequence(Attack attackSequence)
    {
        KeyUI.instance.ClearKeys();
        KeyUI.instance.SetGhostMode(false);
        currentAttack = attackSequence;
        foreach (AttackType attack in currentAttack.attacks)
        {
            currentSequence = "";
            currentSequence += GetKeysForAttack(attack);

            KeyUI.instance.InstantiateNewRow(attack, isNewAttack: !learnedAttacks.Contains(attack));
        }
        
        //After all objects are created, start timer
        //Invoke("StartRecall", memorisationTime);
        StartRecall();
    }
    
    //Called after 3 seconds
    void StartRecall()
    {
        KeyUI.instance.ClearKeys();
        KeyUI.instance.RecallRow(0, currentAttack);
        currentSequence = GetKeysForAttack(currentAttack.attacks[0]);

        //Set "RecallPhase" flag to true
        //Set the current key id to 0 (we start counting from 0)
        //For example if our currentSequence was "ABCDE", 0 would be A, 1 would be B, etc.
        recallPhase = true;
        currentAttackId = 0;
        currentKeyId = 0;
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
                    KeyUI.instance.SpawnKeyObject(currentSequence[currentKeyId].ToString());
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
                        KeyUI.instance.SetRowGreen(currentAttackId);
                        currentAttackId++;

                        if (currentAttackId != currentAttack.attacks.Length)
                        {
                            currentKeyId = 0;
                            KeyUI.instance.RecallRow(currentAttackId, currentAttack);
                            currentSequence = GetKeysForAttack(currentAttack.attacks[currentAttackId]);
                            //For each child of the KeyContainer object
                        }
                        else
                        {
                            PlayerManager.instance.enemyAnimator.SetBool("Defeated",true);
                            recallPhase = false;
                            PlayerManager.instance.playerLocomotionManager.canMove = true; //temporary i think? -JR
                            attackIndex = 0;

                            KeyUI.instance.ClearKeys(clearAll: true, 0.5f);
                        }
                    }
                }
                //If the key is not the one that we want
                else
                {
                    Debug.Log("INCORRECT");
                    PlayerHealth.instance.InflictDamage(amount: 1);
                }
            }
        }
    }

    private void PerformAttackAnimation()
    {
        string attackToPerform = currentAttack.attacks[attackIndex].ToString();
        PlayerManager.instance.playerAnimatorManager.PlayAttackAnimation(attackToPerform);
        AudioManager.instance.PlaySound(attackToPerform, volume: 0.5f);
     
    }
    private void PerformEnemyReaction()
    {
        string attackToPerform = currentAttack.reactions[attackIndex].ToString();
        PlayerManager.instance.enemyAnimator.Play(attackToPerform);
  
    }
    public void NextSequence()
    {
        KeyUI.instance.ClearKeys();
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
