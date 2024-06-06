using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerController : MonoBehaviour
{
    [SerializeField] 
    
    private float TimeMax;

    private float CurrentTime;

    private GameObject objectPoints;

    private bool TimeActivating = false;

    private void Update()
    {
        if (TimeActivating)
        {
            ChangeTimer();
        }
    }


    private void Start()
    {
        objectPoints = GameObject.FindGameObjectWithTag("PointManager");
    }
    private void ChangeTimer()
    {
       CurrentTime -= Time.deltaTime;
        Debug.Log(CurrentTime);

        if (CurrentTime <= 0)
        {
            Debug.Log("Time run out");
            ChangeTempo(false);
            objectPoints.GetComponent<Points>().points += 50;
        
        }
    }

    private void ChangeTempo(bool state)
    { 
      TimeActivating = state;
        
    }

    public void ActivateTempo()
    {
        CurrentTime = TimeMax;
        ChangeTempo(true);
    }

    public void DeactivateTempo()
    {
        ChangeTempo(false);
        objectPoints.GetComponent<Points>().points -= 50;
     
    }
}
