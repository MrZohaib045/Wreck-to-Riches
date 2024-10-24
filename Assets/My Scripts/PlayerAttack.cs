using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerAttack : MonoBehaviour
{
    public AudioSource playerhurt, playerdeathsound, collect, allalphacollected, levelfail;
    public Image player_health;
    ThirdPersonCharacter tpsChar;
    public GameObject[] rayCastPoints, petrolenemy;
    public Image[] slot;
    RaycastHit hit;
    Shooters shooter;
    public float getFilamont;
    PlayerAttack playerattack;
    public static int enmyAtkConter;
    public static bool die;
    public int total_letters;
    int levelclear;
    int nextAvailableSlot;
    Grapical_User_Interface gui;
    // Alpha alpha;
    // rotation r; 
    Alphabetrot alpha;
    EnemyAI enemyAI;
    Clock clock;

   // public Image collectedItemDisplay;
    void Start()
    {
        playerhurt.volume = PlayerPrefs.GetFloat("Sound");
        playerdeathsound.volume = PlayerPrefs.GetFloat("Sound");
        collect.volume = PlayerPrefs.GetFloat("Sound");
        allalphacollected.volume = PlayerPrefs.GetFloat("Sound");
        levelfail.volume = PlayerPrefs.GetFloat("Sound");
        enemyAI = FindObjectOfType<EnemyAI>();
        nextAvailableSlot = 0;
        die = false;
        levelclear = 1;
        player_health.fillAmount = 1;
        enmyAtkConter = 1;
        tpsChar = FindObjectOfType<ThirdPersonCharacter>();
        shooter = FindObjectOfType<Shooters>();
        playerattack = FindObjectOfType<PlayerAttack>();
        alpha = FindObjectOfType<Alphabetrot>();
        gui = FindObjectOfType<Grapical_User_Interface>();
        clock = FindObjectOfType<Clock>();
        
    }
    void Update()
    {
        if (total_letters <= 0 && levelclear==1)
        {
            petrolenemy = GameObject.FindGameObjectsWithTag("ENEMY");
            for (int i = 0; i < petrolenemy.Length; i++)
            {
                petrolenemy[i].GetComponent<EnemyAI>().followRange = -1;
            }
            StartCoroutine(allalphacollec());
        }
    }

    IEnumerator allalphacollec()
    {
        allalphacollected.GetComponent<AudioSource>().enabled = true;
        gui.showscreen(1);
        Clock.time = 0;
        ThirdPersonCharacter.myanimation = false;
        ThirdPersonCharacter.mainPlayer.GetComponent<Animation>().CrossFade("Victory");
        yield return new WaitForSeconds(6f);
        print("Level Clear");
        gui.showscreen(7);
        levelclear = 0;
        allalphacollected.GetComponent<AudioSource>().enabled = false;
    }
        
    //public void CallPlayerAttack()
    //{
    //    StartCoroutine(Fire());
    //}
    //IEnumerator Fire()
    //{

    //    tpsChar.Attack();
    //    yield return new WaitForSeconds(1.3f);
    //    for (int i = 0; i < rayCastPoints.Length; i++)
    //    {
    //        //{
    //        //    ray = new Ray(rayCastPoints[i].transform.position, rayCastPoints[i].transform.forward);
    //        //}
    //        //if(Physics.Raycast(ray, out hit,25f)
    //        if (Physics.Raycast(rayCastPoints[i].transform.position, rayCastPoints[i].transform.forward, out hit, 20f))
    //        {
    //            if (hit.transform != null)
    //            {
    //                if (hit.collider.gameObject.tag.Equals("Ditecable"))
    //                {

    //                    hit.collider.gameObject.GetComponent<Enemy>().enemyhealth.fillAmount -= 0.1f;
    //                    float getFilamont = hit.collider.gameObject.GetComponent<Enemy>().enemyhealth.fillAmount;
    //                    if (getFilamont <= 0)
    //                    {
    //                        GameObject getRagDol = hit.collider.gameObject.GetComponent<Enemy>().Ragdol;
    //                        Instantiate(getRagDol, hit.collider.transform.position, hit.collider.transform.rotation);
    //                        hit.collider.gameObject.SetActive(false);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void CallEnemyAttack()
    {
        if (enmyAtkConter == 1)
        {
            StartCoroutine(EnemyAttack());
            enmyAtkConter = 0;
            
        }
    }
    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(1.2f);
        player_health.fillAmount = player_health.fillAmount - 0.06f;
        playerhurt.GetComponent<AudioSource>().Play();
       // tpsChar.mypain();
        if (player_health.fillAmount <= 0)
        {
            playerdeathsound.GetComponent<AudioSource>().Play();
            ThirdPersonCharacter.wait = 0;
            ThirdPersonCharacter.myanimation = false;
            ThirdPersonUserControl.isRotating = false;
            ThirdPersonCharacter.mainPlayer.CrossFade("Die");
            die = true;
            ThirdPersonCharacter.wait = 0;
            enemyAI.followRange = -1;
            levelfail.GetComponent<AudioSource>().enabled = true;
            petrolenemy = GameObject.FindGameObjectsWithTag("ENEMY");
            for (int i = 0; i < petrolenemy.Length; i++)
            {
               // myenemies[i].GetComponent<Enemy>().fardist = -1;
                petrolenemy[i].GetComponent<EnemyAI>().followRange = -1;
            }
            gui.showscreen(1);
            yield return new WaitForSeconds(5f);
            levelfail.GetComponent<AudioSource>().enabled = false;
            gui.showscreen(3);


            //  print("Player Is Dead!!!!");
        }
        else
        {
            enmyAtkConter = 1;
        }

    }


    private void OnCollisionEnter(Collision obj)
    {

        if (obj.collider.gameObject.CompareTag("food"))
        {
            Debug.Log("healthincrease");
            collect.GetComponent<AudioSource>().Play();
            Destroy(obj.collider.gameObject);
            player_health.fillAmount += 0.3f;
        }
        if (obj.collider.gameObject.CompareTag("Clock"))
        {
            Debug.Log("timeincrease");
            clock.countdown += 60;
            collect.GetComponent<AudioSource>().Play();
            Destroy(obj.collider.gameObject);
         //   countdown -= Time.deltaTime;
        }
        if (obj.collider.gameObject.CompareTag("Alphabets"))
        {
            collect.GetComponent<AudioSource>().Play();
            if (nextAvailableSlot < slot.Length)
            {
                Transform panelManagerTransform = slot[nextAvailableSlot].transform;
                Image firstChild = panelManagerTransform.GetChild(0).GetComponent<Image>();

                if (firstChild != null && firstChild.sprite == null)
                {
                    print("Yes");
                    firstChild.sprite = obj.gameObject.GetComponent<Alphabetrot>().s;
                    nextAvailableSlot++; // Move to the next slot
                }
            }
           // obj.gameObject.GetComponent<AudioSource>().Play();
           // obj.gameObject.GetComponent<AudioSource>().Play();
            //obj.gameObject.GetComponent<AudioSource>().enabled = true;

            print("Alphbets");
            Destroy(obj.collider.gameObject);

            total_letters -= 1;


        }

        // void DisplayCollectedItemSprite()
        //{
        //    if (collectedItemDisplay != null)
        //    {
        //        // Set the collected item sprite to "letters"
        //        collectedItemDisplay.sprite = r.letters;

        //    }
        //}
    }
}