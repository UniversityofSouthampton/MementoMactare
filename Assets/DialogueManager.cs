using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] public GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogueContainer;
    [SerializeField] TextMeshProUGUI nameContainer;
    Coroutine textAnimationCoroutine;

    [Header("Parameters")]
    [SerializeField] float textSpeed = 0.02f;
    [SerializeField] float dialogueCloseTime = 4f;
    [SerializeField] private float dialogueCloseTimer = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        dialogueContainer = GetComponentInChildren<TextMeshProUGUI>();
        DontDestroyOnLoad(gameObject);

        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        HandleDialogueClosing();
    }

    private void HandleDialogueClosing()
    {
        if (!dialogueBox.activeSelf)
            return;

        dialogueCloseTimer += Time.deltaTime;
        if (dialogueCloseTimer > dialogueCloseTime)
        {
            dialogueBox.SetActive(false);
            dialogueCloseTimer = 0;
        }
    }
    public void SetDialogue(string dialogue, CutsceneSettings settings)
    {
        dialogueContainer.text = dialogue;

        if (textAnimationCoroutine == null)
            textAnimationCoroutine = StartCoroutine(TypewriterTextEffect(dialogue, settings));
    }

    public void SetName(string name)
    {
        nameContainer.text = name;
    }
    private IEnumerator TypewriterTextEffect(string dialogue, CutsceneSettings settings)
    {
        dialogueBox.SetActive(true);
        dialogueCloseTimer = 0;
        dialogueContainer.maxVisibleCharacters = 0;

        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogueContainer.maxVisibleCharacters += 1;
            if (char.IsPunctuation(dialogue[i]))
            {
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(2.5f);

        settings.textDoneAnimating = true;
        textAnimationCoroutine = null;

        yield return null;
    }

    public void SkipAndFillCurrentDialogue()
    {
        if (textAnimationCoroutine != null)
            StopCoroutine(textAnimationCoroutine);
        textAnimationCoroutine = null;
        dialogueContainer.maxVisibleCharacters = 999;
    }
}
