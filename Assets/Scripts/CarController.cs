using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider rearRight;
    [SerializeField] WheelCollider rearLeft;

    private Transform TFrontRight;
    private Transform TFrontLeft;
    private Transform TRearRight;
    private Transform TRearLeft;

    private float acceleration = 1500f;
    private float brakingForce = 2000f;
    private float maxTurnAngle = 30f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    private GameObject player = null;
    private PlayerController playerController = null;

    void Start()
    {
        this.TFrontRight = this.transform.Find("WheelMeshes/FR").transform;
        this.TFrontLeft = this.transform.Find("WheelMeshes/FL").transform;
        this.TRearRight = this.transform.Find("WheelMeshes/RR").transform;
        this.TRearLeft = this.transform.Find("WheelMeshes/RL").transform;
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

        this.currentAcceleration = this.acceleration * vInput;

        if (Input.GetKey(KeyCode.Space) && this.player != null)
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
        this.currentTurnAngle = this.maxTurnAngle * hInput;
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

    public void setPlayer(GameObject player)
    {
        this.player = player;
        this.playerController = this.player.GetComponent<PlayerController>();
    }
}
