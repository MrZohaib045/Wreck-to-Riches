using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class swapkro : MonoBehaviour
{
    public GameObject[] letters;
    public GameObject[] dummyletters;
    private Vector3[] lettersInitialPositions;
    public AudioSource[] letterAudioSources;
    public AudioSource Word_sound;
    public AudioSource Wrong_place;

    void Start()
    {
        lettersInitialPositions = new Vector3[letters.Length];
        for (int i = 0; i < letters.Length; i++)
        {
            lettersInitialPositions[i] = letters[i].transform.position;
        }
    }

    public void DragLetter(int letterIndex)
    {
        letters[letterIndex].transform.position = Input.mousePosition;
    }

    public void DropLetter(int letterIndex)
    {
        float distance = Vector3.Distance(letters[letterIndex].transform.position, dummyletters[letterIndex].transform.position);
        if (distance < 200)
        {
            letters[letterIndex].transform.position = dummyletters[letterIndex].transform.position;
            Debug.Log("Letter Dropped Successfully.");

            PlayAudio(letterAudioSources[letterIndex]);

            bool allLettersDropped = true;
            for (int i = 0; i < letters.Length; i++)
            {
                distance = Vector3.Distance(letters[i].transform.position, dummyletters[i].transform.position);
                if (distance >= 200)
                {
                    allLettersDropped = false;
                    break;
                }
            }

            if (allLettersDropped)
            {
                Invoke("playwithsomedelay", 0.45f);
                Invoke("LoadNextScene", 2f);
            }
        }
        else
        {
            vibrate();
            Wrong_place.Play();
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void vibrate()
    {
        Handheld.Vibrate();
    }
}
