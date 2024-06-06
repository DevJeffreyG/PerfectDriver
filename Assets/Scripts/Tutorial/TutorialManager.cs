using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private bool ended = false;
    private Profile profile;

    private void Start()
    {
        profile = ProfileController.getProfile();
    }

    void Update()
    {
        if ((bool) profile.getData(Profile.ProfileData.CompletedTutorial))
        {
            Debug.Log("Completó el tutorial");
            Debug.Log(profile.getName());
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {

                popUps[i].SetActive(true);

            }
            else
            {

                popUps[i].SetActive(false);

            }
        }

        if (popUpIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 4)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 5)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                popUpIndex++;
                ended = true;
            }
        }

        if(ended)
        {
            ProfileController.getProfile().setData(Profile.ProfileData.CompletedTutorial, true);
            Destroy(gameObject);
        }
    }
 }

