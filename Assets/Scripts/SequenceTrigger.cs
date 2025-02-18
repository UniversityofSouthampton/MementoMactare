using System;
using UnityEngine;

public class SequenceTrigger : MonoBehaviour
{
    public KeyGameplay keyGameplayScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.name == "Prototype Player")
        {
            //start next sequence
            keyGameplayScript.NextSequence();
        }
    }
}
