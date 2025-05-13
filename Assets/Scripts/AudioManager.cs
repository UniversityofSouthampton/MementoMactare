using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;

    private Dictionary<String, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();

    public static AudioManager instance;
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
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //load clips into a dictionary for fast-access by clip name
        foreach (AudioClip clip in audioClips)
        {
            clipDictionary.Add(clip.name, clip);
        }
    }

    public void PlaySound(string audioClipName, float volume = 1, float pitch = 1)
    {
        if (!clipDictionary.ContainsKey(audioClipName))
        {
            Debug.Log("Clip not found. Make sure it is added on the AudioManager object and the supplied name is correct (case-sensitive)");
            return;
        }
        AudioSource source = this.gameObject.AddComponent<AudioSource>();
        source.clip = clipDictionary[audioClipName];
        source.volume = volume;
        source.pitch = pitch;

        source.Play();
        Destroy(source, source.clip.length+1);
        
    }





}
