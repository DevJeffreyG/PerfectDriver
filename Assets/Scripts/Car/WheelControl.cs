using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public bool steerable; // Puede girar
    public bool motorized; // Acelera/desacelera

    private WheelCollider wheelCollider;
    public Transform wheelMesh;

    private void Start()
    {
        this.wheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        this.wheelMesh.SetPositionAndRotation(position, rotation);
    }


    public bool canTurn() { return this.steerable; }

    public bool canAccelerate() { return this.motorized; }

    public void setTurnAngle(float angle)
    {
        this.wheelCollider.steerAngle = angle;
    }

    public void accelerate(float torque)
    {
        this.wheelCollider.motorTorque = torque;
    }

    public void setBrakeTorque(float torque)
    {
        this.wheelCollider.brakeTorque = torque;
    }

    public void setMotorTorque(float torque)
    {
        this.wheelCollider.motorTorque = torque;
    }

    public bool isGrounded()
    {
        return this.wheelCollider.isGrounded;
    }
}
