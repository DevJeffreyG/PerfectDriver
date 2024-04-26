using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private WheelCollider frontRight;
    private WheelCollider frontLeft;
    private WheelCollider rearRight;
    private WheelCollider rearLeft;

    private Transform TFrontRight;
    private Transform TFrontLeft;
    private Transform TRearRight;
    private Transform TRearLeft;

    private float acceleration = 1000f;
    private float brakingForce = 2000f;
    private float maxTurnAngle = 30f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    void Start()
    {
        this.frontRight = GameObject.Find("FrontRightCollider").GetComponent<WheelCollider>();
        this.frontLeft = GameObject.Find("FrontLeftCollider").GetComponent<WheelCollider>();
        this.rearRight = GameObject.Find("RearRightCollider").GetComponent<WheelCollider>();
        this.rearLeft = GameObject.Find("RearLeftCollider").GetComponent<WheelCollider>();

        this.TFrontRight = GameObject.Find("FR").transform;
        this.TFrontLeft = GameObject.Find("FL").transform;
        this.TRearRight = GameObject.Find("RR").transform;
        this.TRearLeft = GameObject.Find("RL").transform;
    }

    private void FixedUpdate()
    {
        this.movementManager();
    }

    private void movementManager()
    {
        this.currentAcceleration = this.acceleration * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
            this.currentBrakeForce = this.brakingForce;
        else
            this.currentBrakeForce = 0f;

        this.frontRight.motorTorque = this.currentAcceleration;
        this.frontLeft.motorTorque = this.currentAcceleration;

        this.frontRight.brakeTorque = this.currentBrakeForce;
        this.frontLeft.brakeTorque = this.currentBrakeForce;
        this.rearRight.brakeTorque = this.currentBrakeForce;
        this.rearLeft.brakeTorque = this.currentBrakeForce;

        // GIROS
        this.currentTurnAngle = this.maxTurnAngle * Input.GetAxis("Horizontal");
        this.frontLeft.steerAngle = this.currentTurnAngle;
        this.frontRight.steerAngle = this.currentTurnAngle;

        this.UpdateWheel(this.frontRight, this.TFrontRight);
        this.UpdateWheel(this.frontLeft, this.TFrontLeft);
        this.UpdateWheel(this.rearRight, this.TRearRight);
        this.UpdateWheel(this.rearLeft, this.TRearLeft);

        //Debug.Log(this.speed);
        //Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward.normalized * 5f * speed, Color.green);
    }

    private void UpdateWheel(WheelCollider collider, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        trans.SetPositionAndRotation(position, rotation); 
    }
}
