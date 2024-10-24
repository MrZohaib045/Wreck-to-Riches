using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testing : MonoBehaviour
{
    public GameObject[] letters;
    public GameObject[] dummyletters1;
    public GameObject[] dummyletters2; // Second drop location
    private Vector3[] lettersInitialPositions;

    // Set the threshold distance for a successful drop
    public float dropThreshold = 75f;

    // Start is called before the first frame update
    void Start()
    {
        lettersInitialPositions = new Vector3[letters.Length];
        for (int i = 0; i < letters.Length; i++)
        {
            lettersInitialPositions[i] = letters[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DragLetter(int letterIndex)
    {
        letters[letterIndex].transform.position = Input.mousePosition;
    }

    public void DropLetter(int letterIndex)
    {
        bool dropped = false;
        for (int i = 0; i < dummyletters1.Length; i++)
        {
            float distance1 = Vector3.Distance(letters[letterIndex].transform.position, dummyletters1[i].transform.position);
            float distance2 = Vector3.Distance(letters[letterIndex].transform.position, dummyletters2[i].transform.position);

            if (distance1 < dropThreshold || distance2 < dropThreshold)
            {
                letters[letterIndex].transform.position = (distance1 < distance2) ? dummyletters1[i].transform.position : dummyletters2[i].transform.position;
                Debug.Log("Letter Dropped Successfully.");
                dropped = true;
                break;
            }
            else
            {
                vibrate();
                letters[letterIndex].transform.position = lettersInitialPositions[letterIndex];
            }
        }
        CheckIfAllLettersDropped();
    }

    public void CheckIfAllLettersDropped()
    {
        bool allLettersDropped = true;
        for (int i = 0; i < letters.Length; i++)
        {
            bool dropped = false;
            for (int j = 0; j < dummyletters1.Length; j++)
            {
                float distance1 = Vector3.Distance(letters[i].transform.position, dummyletters1[j].transform.position);
                float distance2 = Vector3.Distance(letters[i].transform.position, dummyletters2[j].transform.position);

                if (distance1 < dropThreshold || distance2 < dropThreshold)
                {
                    dropped = true;
                    break;
                }
            }
            if (!dropped)
            {
                allLettersDropped = false;
                break;
            }
        }

        if (allLettersDropped)
        {
            Invoke("LoadNextScene", 2f);
        }
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
