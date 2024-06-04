using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sign : MonoBehaviour
{
    public int PointsReceived;
    private GameObject ObjPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "UsableCar")
        {
            ObjPoints.GetComponent<Points>().points += PointsReceived;
            Destroy(gameObject);
        }
    }
    void Awake(){
        ObjPoints = GameObject.FindGameObjectWithTag("PointManager");
    }
    
}
