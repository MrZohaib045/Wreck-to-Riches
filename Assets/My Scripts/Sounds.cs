using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] AudioSource Alphabets_Sounds;
    [SerializeField] AudioSource BackgroundMusic;
    [SerializeField] AudioSource abc_sound;
    public List<AudioClip> char_sounds;

    //public static Sounds Instance;

    void Start()
    {
        BackgroundMusic.Play();
    }
    private void Update()
    {
        abc_sound.volume = PlayerPrefs.GetFloat("Sound");
        Alphabets_Sounds.volume = PlayerPrefs.GetFloat("Sound");
    }
    public void PlaySound(int index)
    {
        StopAbcSound();

        if (index >= 0 && index < char_sounds.Count)
        {
            Alphabets_Sounds.clip = char_sounds[index];
            Alphabets_Sounds.Play();
        }
        else
        {
            Debug.LogError("Index out of range or audio clip not found.");
        }
    }

    public void play_abc()
    {
        abc_sound.Play();
    }

    public void StopAbcSound()
    {
        if (abc_sound.isPlaying)
        {
            abc_sound.Stop();
        }
    }
    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
