using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroydeadbody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroydeadbody());
    }
    IEnumerator destroydeadbody()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);

    }
}
