using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;


    void Update()
    {
        for (int i = 0; i < popUps.Length; i++) {
            if (i == popUpIndex) {

                popUps[i].SetActive(true);

            } else {
                
                popUps[i].SetActive(false);
            
            }
        }

        if (popUpIndex == 0) 
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                popUpIndex++;
            }

        } else if (popUpIndex == 1)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) { 
                popUpIndex++; 
            }
            else if (popUpIndex == 2)
            {

            }
        }
    }
}
