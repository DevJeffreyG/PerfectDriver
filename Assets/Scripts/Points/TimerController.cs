using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] 
    
    private float TimeMax;

    private float CurrentTime;

    private bool TimeActivating = false;

    private void Update()
    {
        if (TimeActivating)
        {
            ChangeTimer();
        }
    }

    private void ChangeTimer()
    {
       CurrentTime -= Time.deltaTime;

        if (CurrentTime <= 0)
        {
            Debug.Log("Time run out");
            ChangeTempo(false);
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
    }
}
