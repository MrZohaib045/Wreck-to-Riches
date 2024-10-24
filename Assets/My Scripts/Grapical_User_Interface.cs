using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grapical_User_Interface : MonoBehaviour
{
    public Text level;
    public GameObject enemy;
    public GameObject panel;
    public GameObject[] screens;// Reference to your panel GameObject
    int isloading;
    public Image loadingbar;
    public AudioSource Thingsbuttonsound;
    Clock clock;
    changekr ck;
  // public GameObject[] petrolenemy;

    
    private void Start()
    {
        ck = FindObjectOfType<changekr>();
        clock = FindObjectOfType<Clock>();
        StartCoroutine(startscene());
        isloading = 0;

        for (int i = 0; i < screens.Length; i++)
        {
            if (i > 1)
            {
                if (screens[i].GetComponent<AudioSource>())
                {
                    screens[i].GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music");
                }
            }
            screens[0].GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Sound");
            Thingsbuttonsound.volume = PlayerPrefs.GetFloat("Sound");
        }

          //  ck.audiosource.volume = PlayerPrefs.GetFloat("Sound");
        
    }

    private void Update()
    {
        if (isloading == 1)
        {
            loadingbar.fillAmount += 0.025f;
            if (loadingbar.fillAmount >= 0.99f)
            {
                SceneManager.LoadScene(0);
                isloading = 0;
            }
        }

        if (isloading == 2)
        {
            loadingbar.fillAmount += 0.025f;
            if (loadingbar.fillAmount >= 0.99f)
            {
                isloading = 0;
                int levelToLoad = PlayerPrefs.GetInt("Levelis");
                SceneManager.LoadScene(levelToLoad);
            }
        }

        if (isloading == 3)
        {
            loadingbar.fillAmount += 0.025f;
            if (loadingbar.fillAmount >= 0.99f)
            {
                isloading = 0;
                PlayerPrefs.SetInt("Levelis",1);
                SceneManager.LoadScene(1);
            }
        }
    }
    public void showscreen(int val)
    {
        //        screenVal = val;
        for (int i = 0; i < screens.Length; i++)
        {
            if (i == val)
            {
                screens[i].SetActive(true);
            }
            else
            {
                screens[i].SetActive(false);
            }
        }
        if (val == 0)
        {
            Clock.time = 1;
            enemy.SetActive(true);

        }

        else if (val == 4)
        {
            Clock.time = 0;
            enemy.SetActive(false);
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void loadnextlevel()
    {
        isloading = 2;
    }

    public void loadnextlevel7()
    {
        isloading = 3;
    }
    // This method is called when the button is clicked
    public void TogglePanel()
    {
        // Check if the panel is currently active
        bool isActive = panel.activeSelf;

        // Toggle the panel's visibility
        panel.SetActive(!isActive);
    }

    IEnumerator startscene()
    {
        showscreen(1);
        yield return new WaitForSeconds(6f);
        Clock.time = 1;
        Destroy(level);
        showscreen(0);
    }

    public void backtomenu()
    {
        showscreen(6);
        isloading = 1;
    }

    public void backmenu7()
    {
        showscreen(6);
        isloading = 1;
        PlayerPrefs.SetInt("Levelis", 1);
    }
}