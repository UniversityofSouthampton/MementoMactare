using System.Collections;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager instance;
    //Set up GhostManager as a singleton
    //(https://techhub.wsagames.com/guides//1_Unity/1_Coding/1_Advanced-CSharp#programming-design-patterns)

    private Animator ghostAnimator;
    private SpriteRenderer ghostSpriteRenderer;

    private const float GHOST_FADE_TIME = 0.5f;

    [HideInInspector]
    public bool inSequence;
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ghostAnimator = GetComponent<Animator>();
        ghostSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGhostSequence(Attack attack)
    {
        StartCoroutine(GhostSequenceCoroutine(attack));
    }

    IEnumerator GhostSequenceCoroutine(Attack attack)
    {
        inSequence = true;
        ////////////////////////////////////// Ghost Fade in
        transform.position = PlayerManager.instance.transform.position;
        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / GHOST_FADE_TIME;
            ghostSpriteRenderer.color = new Color(1, 1, 1, f);
            yield return new WaitForEndOfFrame();
        }
        f = 1;

        int attackIndex = 0;
        KeyUI.instance.SetGhostMode(true);
        foreach (AttackType attackType in attack.attacks)
        {
            ////////////////////////////////////// Ghost Show Keys
            KeyUI.instance.InstantiateNewRow(attackType, isNewAttack: true);
            ////////////////////////////////////// Ghost Attack Animation
            PlayAnimation(attackType.ToString());
            AudioManager.instance.PlaySound(attackType.ToString(), volume: 0.5f);
            yield return new WaitForSeconds(0.75f);
            attackIndex++;
        }

        ////////////////////////////////////// Ghost Fade Out
        while (f > 0)
        {
            f -= Time.deltaTime / GHOST_FADE_TIME;
            ghostSpriteRenderer.color = new Color(1, 1, 1, f);
            yield return new WaitForEndOfFrame();
        }
        KeyUI.instance.ClearKeys(clearAll: true);
        yield return null;
        inSequence = false;
    }

    private void PlayAnimation(string attackAnimation)
    {
        ghostAnimator.Play(attackAnimation);
    }
}
