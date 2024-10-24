using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotonext : MonoBehaviour
{
    public Sounds sound_off;
    public GameObject abc_panel;
    public GameObject abc_panel2;

    public void Start()
    {
        sound_off = GameObject.FindObjectOfType<Sounds>();
       // DontDestroyOnLoad(abc_panel2);
    }

    public void load_next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void onclick()
    {
        if (abc_panel.activeInHierarchy == false)
        {
            abc_panel.SetActive(true);
            abc_panel2.SetActive(false);
        }
        else
        {
            abc_panel.SetActive(false);
            abc_panel2.SetActive(true);
        }
    }

    public void cross()
    {
        sound_off.StopAbcSound();

        if (abc_panel2.activeInHierarchy == false)
        {
            abc_panel2.SetActive(true);
            abc_panel.SetActive(false);
        }
        else
        {
            abc_panel2.SetActive(false);
            abc_panel.SetActive(true);
        }

    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("you are going to quit");
    }
}
