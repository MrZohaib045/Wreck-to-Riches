using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changekr : MonoBehaviour
{
    public GameObject currentPanel;
    public GameObject newPanel;
    public float delay;
    public AudioSource audiosource;

    private void Start()
    {
        audiosource.volume = PlayerPrefs.GetFloat("Sound");
    }
    public void OnClickSwitchButton()
    {
        StartCoroutine(SwitchPanelsWithDelay());

    }

    IEnumerator SwitchPanelsWithDelay()
    {
        currentPanel.SetActive(false);
        newPanel.SetActive(true);
        audiosource.Play();
        yield return new WaitForSeconds(delay);
        newPanel.SetActive(false);
        currentPanel.SetActive(true);
    }
}