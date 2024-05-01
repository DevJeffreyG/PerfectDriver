using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager getManager()
    {
        return GameObject.Find("SceneManagerC").GetComponent<SceneManager>();
    }

    public void SceneSelector(string scene){
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
