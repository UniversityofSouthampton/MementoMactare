using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;

    [SerializeField] private Dictionary<String, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();

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
        DontDestroyOnLoad(this.gameObject); //so level transition audio does not cut out

        //load clips into a dictionary for fast-access by clip name
        foreach (AudioClip clip in audioClips)
        {
            clipDictionary.Add(clip.name, clip);
        }
    }

    public void CheckSFXAlreadyPlaying(string name)
    {
        foreach (AudioSource audioSource in gameObject.GetComponents<AudioSource>())
        {
            if (audioSource.clip == GetClipFromDictionary(name))
            {
                Destroy(audioSource);
            }
        }
    }

    public AudioClip GetClipFromDictionary(string name)
    {
        if (clipDictionary.ContainsKey(name)) return clipDictionary[name];

        return null;
    }
    public void PlaySound(string audioClipName, float volume = 1, float pitch = 1)
    {
        if (!clipDictionary.ContainsKey(audioClipName))
        {
            Debug.Log(
                "Clip not found. Make sure it is added on the AudioManager object and the supplied name is correct (case-sensitive)");
            return;
        }

        //foreach (AudioSource audioSource in gameObject.GetComponents<AudioSource>())
        //{
            //if (audioSource.clip == clipDictionary[audioClipName])
            //{
               // Destroy(audioSource);
            //}
        //}

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clipDictionary[audioClipName];
        source.volume = volume;
        source.pitch = pitch;

        source.Play();
        Destroy(source, source.clip.length + 1);
    }
}