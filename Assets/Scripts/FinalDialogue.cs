using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinalDialogue : MonoBehaviour
{
    CutsceneSettings settings;

    [SerializeField] private PlayableDirector playableDirector; 

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
    private bool done = false;
    private void Start()
    {
    }

    private void Update()
    {
        if (done) return;
        if (!sequencePlaying)
        {
            settings = GetComponent<CutsceneSettings>();
            sequencePlaying = true;
            settings.textDoneAnimating = true;
            playableDirector.Play();
        }
        
        if (!sequencePlaying)
            return;
        if (!settings.textDoneAnimating)
            return;
        PlayNextDialogue();
    }

    public void PlayNextDialogue()
    {
        settings.textDoneAnimating = false;
        dialogueIndex++;
        if (dialogueIndex == finalDialogue.Count)
        {
            done = true;
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
