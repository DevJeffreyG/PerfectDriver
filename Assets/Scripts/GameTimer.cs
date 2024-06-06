using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    private float time = 10;
    private GameObject pointManager;
    private Profile profile;
    private void Start()
    {
        profile = ProfileController.getProfile();
        pointManager = GameObject.FindGameObjectWithTag("PointManager");

        profile.setData(Profile.ProfileData.TimesPlayed, ((int)profile.getData(Profile.ProfileData.TimesPlayed)) + 1);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;


        if(time < 0 )
        {
            int points = pointManager.GetComponent<Points>().points;

            if(points > (int) profile.getData(Profile.ProfileData.MaxPoints))
            {
                ProfileController.getProfile().setData(Profile.ProfileData.MaxPoints, points);
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOver");
        }
    }
}
