using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    void Update()
    {
        if(!GameObject.Find("TutorialManager"))
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
