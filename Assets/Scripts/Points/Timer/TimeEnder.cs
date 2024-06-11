using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnder : MonoBehaviour
{
    private TimerController timerController;

    void Awake()
    {
        timerController = GameObject.FindGameObjectWithTag("TimerManager").GetComponent<TimerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarCollider"))
        {
            timerController.DeactivateTempo();
        }
    }
}
