using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerControl : MonoBehaviour
{
    public static SceneManagerControl getManager()
    {
        return GameObject.Find("SceneManagerC").GetComponent<SceneManagerControl>();
    }

    public void SceneSelector(string scene){
        SceneManager.LoadScene(scene);
    }
}
