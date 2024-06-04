using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBridge : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        // TODO: Revisar si debe ver el tutorial o no
        SceneManagerControl manager = SceneManagerControl.getManager();
        manager.SceneSelector("Tutorial");
    }
}
