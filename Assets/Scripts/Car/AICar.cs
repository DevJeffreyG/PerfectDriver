using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform carFront;
    [SerializeField] private Transform carRear;

    private Vector3 targetPos;
    private Rigidbody carPhysics;
    private WheelControl[] wheels;

    private float maxTurnAngle = 30f;
    private float maxTorque = 2000f;
    private float brakeTorque = 3000;

    private float torque;
    private float ang;
    private bool stopping = false;
    // Start is called before the first frame update
    void Start()
    {
        carPhysics = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelControl>();

        // Ajusta el centro de masa del carro para evitar que pasen cosas raras
        carPhysics.centerOfMass += Vector3.up * -1f;
    }

    // Update is called once per frame
    void Update()
    {
        this.setTargetPos(target.position);

        Vector3 toMovePosition = (this.targetPos - transform.position).normalized;
        float fwOrBck = Vector3.Dot(transform.forward, toMovePosition);
        Transform relativeReference = fwOrBck >= 0 ? carFront : carRear;
        //Debug.Log(relativeReference.gameObject);

        float distanceTillTarget = Vector3.Distance(relativeReference.position, this.targetPos);
        float speed = carPhysics.velocity.magnitude;
        float vAmount;
        float hAmount;
        
        if (fwOrBck > 0) // El destino está adelante
        {
            vAmount = 1f;
        }
        else // El destino está detrás
        {
            vAmount = -1f;
        }

        float deacc = 1.551f; // Muchas cosas pasaron para llegar a esto. https://www.calculatorsoup.com/calculators/physics/velocity-calculator-vuas.php
        float stoppingDistance = Mathf.Pow(Conversor.UnitsToMeters(speed), 2) / (2 * (deacc));
        if (/*speed > Conversor.MetersToUnits(3)*/ distanceTillTarget < Conversor.MetersToUnits(stoppingDistance) && !stopping) // Si está llegando al destino, empezar a frenar
        {
            toggleStopping();
            Debug.Log(Conversor.UnitsToMeters(distanceTillTarget));
            Debug.Log(Conversor.UnitsToMeters(speed));
        }

        float angle = Vector3.SignedAngle(relativeReference.forward, toMovePosition, Vector3.up);

        if (angle > 0f) // Derecha
        {
            hAmount = 1f;
        }
        else
        {
            hAmount = -1f;
        }

        movementController(vAmount, hAmount);
    }

    public void setTargetPos(Vector3 pos)
    {
        this.targetPos = pos;
    }

    private void movementController(float vertical, float horizontal)
    {
        float forwardSpeed = Vector3.Dot(transform.forward, carPhysics.velocity); 
        bool isAccelerating = Mathf.Sign(vertical) == Mathf.Sign(forwardSpeed) && !stopping;

        if(carPhysics.velocity.magnitude < 0.9f && stopping)
        {
            carPhysics.velocity = Vector3.zero;
            Debug.Log(Conversor.UnitsToMeters(Vector3.Distance(carFront.position, this.targetPos)));

            //stopping = false;
        }

        foreach(WheelControl wheel in wheels)
        {
            if(isAccelerating)
            {
                //Debug.Log("ACELERANDO");

                if (wheel.canAccelerate())
                {
                    wheel.accelerate(maxTorque * vertical);
                }

                wheel.setBrakeTorque(0);
            } else // Esta frenando
            {
                //Debug.Log("FRENANDO");

                wheel.setMotorTorque(0);
                wheel.setBrakeTorque(brakeTorque);
            }

            if(wheel.canTurn() && horizontal != 0f)
            {
                wheel.setTurnAngle(maxTurnAngle * horizontal);
            }
        }
    }

    public void toggleStopping()
    {
        stopping = !stopping;
    }
}
