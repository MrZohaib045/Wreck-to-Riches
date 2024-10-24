using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemyhealth : MonoBehaviour
{
    public Image enemyhealth;
    public GameObject Ragdol;
    public AudioSource enemypain;
    // Start is called before the first frame update
    void Start()
    {
        enemypain.volume = PlayerPrefs.GetFloat("Sound");
        Ragdol.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Sound");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ThirdPersonCharacter.attackison == true)
        {


            if (collision.collider.gameObject.CompareTag("weapon"))
            {
                Debug.Log("enemy");

                enemyhealth.fillAmount -= 0.4f;
                if (enemyhealth.fillAmount <= 0)
                {
                    Instantiate(Ragdol, transform.position, transform.rotation);
                    //Ragdol.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Sound");
                    Destroy(this.gameObject);
                }
            }
        }
    }

}
