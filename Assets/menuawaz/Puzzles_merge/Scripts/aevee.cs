using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aevee : MonoBehaviour
{
    [SerializeField] AudioSource chankana;
    void Start()
    {
        chankana.Play();
    }
}
