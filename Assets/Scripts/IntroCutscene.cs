using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    CutsceneSettings settings;

    [Serializable] public struct DialogueLine
    {
        public string characterName;
        public string line;
    }
    [Header("Intro Cutscene Dialogue")]
    [SerializeField] private List<DialogueLine> introDialogue;

    private int currentDialogueIndex;
    private bool Triggered;
    private bool sequencePlaying;

    private void Start()
    {
        settings = GetComponent<CutsceneSettings>();
    }

    private void Update()
    {
        if (!sequencePlaying)
            return;
        if (!settings.textDoneAnimating)
            return;
        DoIntroDialogue();
    }

    void SequenceEnd()
    {
        sequencePlaying = false;
        PlayerManager.instance.playerLocomotionManager.canMove = true;
        DialogueManager.instance.dialogueBox.SetActive(false);
    }
    void PlayIntroSequence()
    {
        sequencePlaying = true;
        settings.textDoneAnimating = true;
        //DialogueManager.instance.SetName(introDialogue[0].characterName);
    }
    void DoIntroDialogue()
    {
        if (!settings.textDoneAnimating)
            return;
        settings.textDoneAnimating = false;

        if (currentDialogueIndex < introDialogue.Count)
        {
            DialogueManager.instance.dialogueBox.SetActive(true);
            DialogueManager.instance.SetName(introDialogue[currentDialogueIndex].characterName);
            DialogueManager.instance.SetDialogue(introDialogue[currentDialogueIndex].line, settings);
            currentDialogueIndex += 1;
        }
        else
        {
            SequenceEnd();
            DialogueManager.instance.dialogueBox.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (Triggered)
            return;
        if (other.tag == "Player")
        {
            Triggered = true;

            PlayIntroSequence();
            PlayerManager.instance.playerLocomotionManager.canMove = false;
        }
    }
}
