using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class swap : MonoBehaviour
{

    public GameObject[] letters;
    public GameObject[] dummyletters;
    private Vector3[] lettersInitialPositions;
    public AudioSource[] letterAudioSources;
    public AudioSource Word_sound;
    public GameObject hide_kro;
    Grapical_User_Interface gui;

    public float Theresh_hold;

    void Start()
    {

        gui = FindObjectOfType<Grapical_User_Interface>();
        lettersInitialPositions = new Vector3[letters.Length];
        for (int i = 0; i < letters.Length; i++)
        {
            lettersInitialPositions[i] = letters[i].transform.position;
        }
        foreach (var audioSource in letterAudioSources)
        {
            audioSource.volume = PlayerPrefs.GetFloat("Sound");
        }
        Word_sound.volume = PlayerPrefs.GetFloat("Sound");
    }

    public void DragLetter(int letterIndex)
    {
        letters[letterIndex].transform.position = Input.mousePosition;
    }

    public void DropLetter(int letterIndex)
    {

        float distance = Vector3.Distance(letters[letterIndex].transform.position, dummyletters[letterIndex].transform.position);
        if (distance < Theresh_hold)
        {
            letters[letterIndex].transform.position = dummyletters[letterIndex].transform.position;
            Debug.Log("Letter Dropped Successfully.");
            PlayAudio(letterAudioSources[letterIndex]);

            bool allLettersDropped = true;
            for (int i = 0; i < letters.Length; i++)
            {
                distance = Vector3.Distance(letters[i].transform.position, dummyletters[i].transform.position);
                if (distance >= Theresh_hold)
                {
                    allLettersDropped = false;
                    break;
                }
            }

            if (allLettersDropped)
            {
                Invoke("playwithsomedelay", 0.45f);
                hide_kro.SetActive(false);
                Invoke("LoadNextScene", 2f);
            }
        }
        else
        {
            vibrate();
            letters[letterIndex].transform.position = lettersInitialPositions[letterIndex];
        }
    }

    void PlayAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void playwithsomedelay()
    {
        Word_sound.Play();
    }
    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        PlayerPrefs.SetInt("Levelis", nextSceneIndex);
        gui.showscreen(2);
    }

    public void vibrate()
    {
        Handheld.Vibrate();
    }
}
