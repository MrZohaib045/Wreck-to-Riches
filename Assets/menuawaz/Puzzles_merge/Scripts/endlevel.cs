using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endlevel : MonoBehaviour
{
    public int sceneIndexToLoad;
    void Start()
    {
        Invoke("LoadSceneWithDelay", 5f);
    }
    void LoadSceneWithDelay()
    {
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
