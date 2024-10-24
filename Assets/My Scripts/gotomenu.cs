using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotomenu : MonoBehaviour
{
    public void load_instant()
    {
        int sceneIndex = 0;
        SceneManager.LoadScene(sceneIndex);
    }
}
