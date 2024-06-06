using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    private CarController car;
    private Points ObjPoints;

    public int MaxSpeed;

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("CarCollider"))    
        {
            float Speed = car.getKmH();
                if(Speed <= MaxSpeed){
                    ObjPoints.points += 50;
                 } else  {
                    ObjPoints.points -= 50;
                  }
            Debug.Log("entr�");
            Destroy(gameObject);
        }
    }
    

   
    void Awake(){
        car = GameObject.FindGameObjectWithTag("UsableCar").GetComponent<CarController>();
        ObjPoints = GameObject.FindGameObjectWithTag("PointManager").GetComponent<Points>();

    }
}
