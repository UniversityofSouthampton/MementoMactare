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

    public void StartGhostSequence()
    {
        StartCoroutine(GhostSequenceCoroutine());
    }

    IEnumerator GhostSequenceCoroutine()
    {
        ////////////////////////////////////// Ghost Fade in
        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / GHOST_FADE_TIME;
            ghostSpriteRenderer.color = new Color(1, 1, 1, f);
            yield return new WaitForEndOfFrame();
        }
        f = 1;
        ////////////////////////////////////// Ghost Show Keys
        
        ////////////////////////////////////// Ghost Attack Animation

        ////////////////////////////////////// Ghost Fade Out
        while (f > 0)
        {
            f -= Time.deltaTime / GHOST_FADE_TIME;
            ghostSpriteRenderer.color = new Color(1, 1, 1, f);
            yield return new WaitForEndOfFrame();
        }
        f = 0;
        yield return null;
    }

    private void PlayAnimation(string attackAnimation)
    {
        ghostAnimator.Play(attackAnimation);
    }
}
