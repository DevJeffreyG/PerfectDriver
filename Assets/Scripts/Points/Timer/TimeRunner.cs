using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRunner : MonoBehaviour
{
    [SerializeField] private TimerController timerController;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("CarCollider"))    
        {
            timerController.ActivateTempo();
            Debug.Log("entró");
        }
    }
}
