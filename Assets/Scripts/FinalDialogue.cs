using System;
using System.Collections.Generic;
using UnityEngine;

public class FinalDialogue : MonoBehaviour
{
    CutsceneSettings settings;

    [Serializable] public struct DialogueLine
    {
        public string characterName;
        public string line;
        public int dialogueLength;
    }
    
    [Header("Final Cutscene Dialogue")]
    [SerializeField] private List<DialogueLine> finalDialogue;

    private int dialogueIndex = -1;
    private bool sequencePlaying = false;
    
    private void Start()
    {
        settings = GetComponent<CutsceneSettings>();
        sequencePlaying = true;
        settings.textDoneAnimating = true;
    }

    private void Update()
    {
        if (!sequencePlaying)
            return;
        if (!settings.textDoneAnimating)
            return;
        PlayNextDialogue();
    }

    public void PlayNextDialogue()
    {
        if (!settings.textDoneAnimating)
            return;
        settings.textDoneAnimating = false;
        dialogueIndex++;
        if (dialogueIndex == finalDialogue.Count)
        {
            sequencePlaying = false;
            return;
        }
        Debug.Log($"Playing dialogue {dialogueIndex}");
        DialogueManager.instance.dialogueBox.SetActive(true);
        DialogueManager.instance.SetCloseTime(finalDialogue[dialogueIndex].dialogueLength);
        DialogueManager.instance.SetName(finalDialogue[dialogueIndex].characterName);
        DialogueManager.instance.SetDialogue(finalDialogue[dialogueIndex].line, settings);
    }
    
    
    
}
