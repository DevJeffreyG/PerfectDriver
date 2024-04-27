using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float motorTorque = 2000f; // Potencia del motor, es ineficiente si se sube mucho sin cambiar propiedades de ruedas
    private float brakeTorque = 2000f; // Cantidad de freno, que tan bien podria frenar
    
    private float maxSpeed = 20f;
    private float turnRange = 30f;
    private float turnRangeAtMaxSpeed = 10f;

    private GameObject player = null;
    private PlayerController playerController = null;

    private Rigidbody carPhysics;
    private WheelControl[] wheels;

    void Start()
    {
        carPhysics = GetComponent<Rigidbody>();

        // Ajusta el centro de masa del carro para evitar que pasen cosas raras
        carPhysics.centerOfMass += Vector3.up * -1f;

        // Busca todas las ruedas que tenga el carro con el componente de WheelControl
        wheels = GetComponentsInChildren<WheelControl>();
    }

    private void FixedUpdate()
    {
        this.movementManager();
        this.keybindsManager();
    }

    private void keybindsManager()
    {
        if (this.player == null) return;
        
        if(Input.GetKeyDown(KeyCode.F)) {
            this.player = null;
            this.playerController.getOutOfCar();
            this.playerController = null;
        }
    }

    private void movementManager()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        if (this.player == null)
        {
            vInput = 0f;
            hInput = 0f;
        }

        // Calcula la velocidad actual
        float forwardSpeed = Vector3.Dot(transform.forward, carPhysics.velocity);

        Debug.Log(forwardSpeed);

        // Calcula que tan cerca está el carro de llegar a su velocidad máxima
        // Con un numero de 0 a 1
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Con ese numero se obtiene cuanto torque está disponible
        // Si speedFactor está en 1 (MAX VELOCIDAD) currentMotorTorque es 0, y viceversa
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // Calcular qué tanto puede girar
        // Porque el carro gira más a máxima velocidad
        float currentSteerRange = Mathf.Lerp(turnRange, turnRangeAtMaxSpeed, speedFactor);

        // Revisar si el jugador está yendo a la misma dirección que el carro en ese instante
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);
        bool usingHandBrake = Input.GetKey(KeyCode.Space) && this.player != null;

        foreach (WheelControl wheel in wheels)
        {
            if(wheel.canTurn())
            {
                wheel.setTurnAngle(hInput * currentSteerRange);
            }

            if (isAccelerating)
            {
                if (wheel.canAccelerate())
                {
                    wheel.accelerate(vInput * currentMotorTorque);
                }

                wheel.setBrakeTorque(0);
            } else
            {
                // El jugador quiere ir en contra de la direccion actual
                // (Frenar)

                wheel.setBrakeTorque(Mathf.Abs(vInput) * brakeTorque);
                wheel.setMotorTorque(0);
            }

            if(usingHandBrake)
            {
                wheel.setBrakeTorque(brakeTorque);
            }
        }

        Debug.DrawLine(this.transform.position + new Vector3(0, 5f, 0), this.transform.position + new Vector3(0, 5f, 0) + this.transform.forward.normalized * forwardSpeed, Color.green);
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;
        this.playerController = this.player.GetComponent<PlayerController>();
    }
}
