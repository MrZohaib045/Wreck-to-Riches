using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject[] myscreens;
    public Slider MusicSlider, SoundSlider;
    public Image loadingbar;
    int isloading;
    gotonext gtn;


    // Start is called before the first frame update
    void Start()
    {

        gtn = FindObjectOfType<gotonext>();
        isloading = 0;
        showscreen(0);
        if (PlayerPrefs.GetInt("Starting") == 0)
        {
            PlayerPrefs.SetInt("Starting", 1);
            PlayerPrefs.SetInt("Levelis", 1);
            //PlayerPrefs.SetInt("levelunlock" + 1, 1);
        }

        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetFloat("Sound", 1);
        }

        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 1);
        }


        SoundSlider.value = PlayerPrefs.GetFloat("Sound");
        MusicSlider.value = PlayerPrefs.GetFloat("Music");
        gtn.sound_off.StopAbcSound();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("Music", MusicSlider.value);
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music");
        PlayerPrefs.SetFloat("Sound", SoundSlider.value);

        if (isloading == 1)
        {
            loadingbar.fillAmount += 0.01f;
            if (loadingbar.fillAmount >= 0.99f)
            {
                isloading = 0;

                int levelToLoad = PlayerPrefs.GetInt("Levelis");
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }

    public void showscreen(int val)
    {
        //        screenVal = val;
        for (int i = 0; i < myscreens.Length; i++)
        {
            if (i == val)
            {
                myscreens[i].SetActive(true);
            }
            else
            {
                myscreens[i].SetActive(false);
            }
        }
    }

    public void LoadSceneFromPlayerPrefs()
    {
        isloading = 1;
        //int levelToLoad = PlayerPrefs.GetInt("Levelis");
        //SceneManager.LoadScene(levelToLoad);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
