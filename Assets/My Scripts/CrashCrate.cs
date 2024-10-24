
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
namespace ArionDigital
{
    using System;
    using UnityEngine;
    using UnityStandardAssets.Characters.ThirdPerson;


    public class CrashCrate : MonoBehaviour
    {
        [Header("Whole Create")]
        public MeshRenderer wholeCrate;
      //  public BoxCollider boxCollider;
        [Header("Fractured Create")]
        public GameObject fracturedCrate;
        [Header("Audio")]
        public AudioSource crashAudioClip; //hitcrate;
        int boxhealth;
        public GameObject BoxRagdol;
        public GameObject weaponhitsound;
        //int box_health;

        private void Start()
        {
            crashAudioClip.volume = PlayerPrefs.GetFloat("Sound");
            weaponhitsound.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Sound");
        }
        private void OnCollisionEnter(Collision other)
        {
            if (ThirdPersonCharacter.attackison == true)
            {

                if (other.collider.gameObject.CompareTag("weapon"))
                {
 
                    weaponhitsound.GetComponent<AudioSource>().Play();
                    
                        wholeCrate.enabled = false;
                        GetComponent<BoxCollider>().enabled = false;
                        fracturedCrate.SetActive(true);
                        crashAudioClip.Play();
                      //  Healthcanvas.SetActive(false);
                        //end
                        //little higher
                        Vector3 newPosition = transform.position + new Vector3(0, 0.6f, 0);
                        //Instantiate(BoxRagdol, transform.position, transform.rotation);
                        Instantiate(BoxRagdol, newPosition, transform.rotation);
                        fracturedCrate.SetActive(true);
                        StartCoroutine(destroycratepieces());
            
                }
            }
        }
        [ContextMenu("Test")]
        public void Test()
        {
            wholeCrate.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }

        IEnumerator destroycratepieces()
        {
            yield return new WaitForSeconds(2f);
            print("destroycrate");
           // Destroy(fracturedCrate);
            fracturedCrate.SetActive(false);

        }
    }
}