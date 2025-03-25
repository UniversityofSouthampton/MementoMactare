using System;
using Unity.VisualScripting;
using UnityEngine;

public class SequenceTrigger : MonoBehaviour
{
    public KeyGameplay keyGameplayScript;

    public Attack enemyAttackSequence;

    private bool Triggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Start()
    {
        keyGameplayScript = KeyGameplay.instance.GetComponent<KeyGameplay>();
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (Triggered)
            return;
        if (other.tag == "Player")
        {
            //start next sequence
            Triggered = true;
            //keyGameplayScript.NextSequence();
            keyGameplayScript.StartSpecificSequence(enemyAttackSequence);
            PlayerManager.instance.playerLocomotionManager.canMove = false;
            PlayerManager.instance.currentEnemy = gameObject;
            PlayerManager.instance.enemyAnimator = gameObject.GetComponent<Animator>();
        }
    }
}
