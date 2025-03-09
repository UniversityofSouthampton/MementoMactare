using System;
using Unity.VisualScripting;
using UnityEngine;

public class SequenceTrigger : MonoBehaviour
{
    public KeyGameplay keyGameplayScript;

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
        Debug.Log(other.name);
        if (Triggered)
            return;
        if (other.name == "Prototype Player")
        {
            //start next sequence
            Triggered = true;
            keyGameplayScript.NextSequence();
            PlayerManager.instance.playerLocomotionManager.canMove = false;
            PlayerManager.instance.currentEnemy = gameObject;
            PlayerManager.instance.enemyAnimator = gameObject.GetComponent<Animator>();
        }
    }
}
