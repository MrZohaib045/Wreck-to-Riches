using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Clock : MonoBehaviour
{
    public Text timeup;
    public GameObject mainplayer;
    public GameObject[] petrolenemy;
    public float countdown;
    public Text Displaycount;
    Grapical_User_Interface gui;
    Enemy enemy;
    public static int time;
    EnemyAI enemyAI;
    PlayerAttack pattck;

    // Start is called before the first frame update
    void Start()
    {
        pattck = FindObjectOfType<PlayerAttack>();
        time = 0;
        gui = FindObjectOfType<Grapical_User_Interface>();
        enemy = FindObjectOfType<Enemy>();
        enemyAI = FindObjectOfType<EnemyAI>();
    }
    void Update()
    {
        if (time == 1)
        {
            countdown -= Time.deltaTime;
            float minutes = countdown / 60;
            float seconds = countdown % 60;
            int minutesInt = (int)minutes;
            int secondsInt = (int)seconds;
            string timerText = string.Format("{0:00}:{1:00}", minutesInt, secondsInt);
            Displaycount.text = timerText;
            if (countdown <= 0)
            {
                countdown = 0;
                time = 0;
                ThirdPersonCharacter.myanimation = false;
                mainplayer.GetComponent<Animation>().CrossFade("Sad");
                StartCoroutine(CounterDownTimerCoroutine());
            }
        }
    }
    IEnumerator CounterDownTimerCoroutine()
    {
        timeup.gameObject.SetActive(true);
        petrolenemy = GameObject.FindGameObjectsWithTag("ENEMY");
        for (int i = 0; i < petrolenemy.Length; i++)
        {
            petrolenemy[i].GetComponent<EnemyAI>().followRange = -1;
        }
        pattck.levelfail.GetComponent<AudioSource>().enabled = true;
        gui.showscreen(1);
        enemyAI.followRange = -1;
      //  enemy.fardist = -1;
        yield return new WaitForSeconds(6f);
        timeup.gameObject.SetActive(false);
        pattck.levelfail.GetComponent<AudioSource>().enabled = false;
        print("Hello");
        
        gui.showscreen(3);
    //   // uiController.dersiredScreen(4);
    }
}