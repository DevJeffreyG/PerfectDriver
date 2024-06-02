using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sign : MonoBehaviour
{
    public GameObject ObjPoints;
    public int PointsReceived;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ObjPoints.GetComponent<Points>().points += PointsReceived;
            Destroy(gameObject);
        }
    }
}
