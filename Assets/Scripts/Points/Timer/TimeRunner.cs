using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRunner : MonoBehaviour
{
    [SerializeField] private TimerController timerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("UsableCar"))    
        {
            timerController.ActivateTempo();        
        }
    }
}
