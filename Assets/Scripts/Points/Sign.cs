using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sign : MonoBehaviour
{
    public int PointsReceived;
    private GameObject ObjPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CarCollider"))
        {
            ObjPoints.GetComponent<Points>().points += PointsReceived;
            Destroy(gameObject);
        }
    }
    void Awake(){
        ObjPoints = GameObject.FindGameObjectWithTag("PointManager");
    }
    
}
