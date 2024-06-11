using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRunner : MonoBehaviour
{
    private TimerController timerController;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("CarCollider"))    
        {
            timerController.ActivateTempo();
            Debug.Log("entr�");
        }
    }
    void Awake(){
        timerController = GameObject.FindGameObjectWithTag("TimerManager").GetComponent<TimerController>();
    }
}
