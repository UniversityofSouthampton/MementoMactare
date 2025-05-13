using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SequenceTrigger : MonoBehaviour
{
    CutsceneSettings settings;
    public KeyGameplay keyGameplayScript;
    public Attack enemyAttackSequence;

    [Header("Enemy Parameters - keep empty unless needed for dialogue")]
    [SerializeField] string enemyName;
    [SerializeField] string introDialoguesOfCurrentEnemy;

    private bool Triggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Start()
    {
        settings = GetComponent<CutsceneSettings>();
        keyGameplayScript = KeyGameplay.instance.GetComponent<KeyGameplay>();
    }
    void PopulateDialogueManager()
    {
        if (string.IsNullOrEmpty(enemyName) || string.IsNullOrEmpty(introDialoguesOfCurrentEnemy))
        {
            GhostManager.instance.StartGhostSequence(enemyAttackSequence);
            StartCoroutine(WaitForGhostSequence());
            return;
        }

        DialogueManager.instance.dialogueBox.SetActive(true);
        DialogueManager.instance.SetName(enemyName);
        DialogueManager.instance.SetDialogue(introDialoguesOfCurrentEnemy, settings); //this automatically does the dialogue as well.

        StartCoroutine(WaitForDialogueToFinishToPlaySequence());
    }

    private IEnumerator WaitForDialogueToFinishToPlaySequence()
    {
        while (DialogueManager.instance.dialogueBox.activeSelf) yield return null;

        GhostManager.instance.StartGhostSequence(enemyAttackSequence);
        StartCoroutine(WaitForGhostSequence());
        yield return null;
    }
    
    private IEnumerator WaitForGhostSequence()
    {
        while (GhostManager.instance.inSequence) yield return null;

        StartSequence();
        yield return null;
    }

    private void StartSequence()
    {
        KeyUI.instance.SetGhostMode(false);
        keyGameplayScript.StartSpecificSequence(enemyAttackSequence);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.tag);
        if (Triggered)
            return;
        if (other.tag == "Player")
        {
            //start next sequence
            Triggered = true;
            //keyGameplayScript.NextSequence();
            PopulateDialogueManager();
            PlayerManager.instance.playerLocomotionManager.canMove = false;
            PlayerManager.instance.currentEnemy = gameObject;
            PlayerManager.instance.enemyAnimator = gameObject.GetComponent<Animator>();
        }
    }
}
