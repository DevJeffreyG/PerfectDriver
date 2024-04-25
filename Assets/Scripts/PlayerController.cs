using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Camera playerCamera;

    public float MovementSpeed = 5f;
    public float JumpSpeed = 2f;
    public float Sensibility = 5f;

    private float CurrentYSpeed = 0f;
    private float cameraRotationX = 0f;
    private float cameraRotationY = 0f;
    private bool isInCar = false;

    private readonly float maxCameraYCar = 30f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = gameObject.GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        this.cameraManager();
        this.movementManager();
    }

    private void movementManager()
    {
        // Si está dentro de un carro el movimiento será distinto (el de un carro)
        if(this.isInCar) return;

        // Toma el sentido hacia donde está viendo la cámara del jugador
        Vector3 forward = this.playerCamera.transform.forward;
        Vector3 right = this.playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        // Con la norma del vector de la camara se puede mover en la dirección relativa a esta
        forward.Normalize();
        right.Normalize();

        // Obtiene el input de la forma horizontal y vertical (flechas, WASD)
        Vector3 move = right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");

        // Si está en el suelo
        if (this.characterController.isGrounded)
        {
            // Si está saltando
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.CurrentYSpeed = this.JumpSpeed;
            }
        }

        this.CurrentYSpeed -= 9.8f * Time.deltaTime;
        move.y = this.CurrentYSpeed;

        this.characterController.Move(this.MovementSpeed * Time.deltaTime * move);
    }

    private void cameraManager()
    {
        this.cameraRotationY += Input.GetAxis("Mouse X") * this.Sensibility;
        this.cameraRotationX += Input.GetAxis("Mouse Y") * -1 * this.Sensibility;

        if (this.cameraRotationX > 90f) this.cameraRotationX = 90f;
        else if (this.cameraRotationX < -90f) this.cameraRotationX = -90f;

        // Si está dentro de un carro, no puede girar tanto por estar sentado
        if(this.isInCar)
        {
            if (this.cameraRotationY > this.maxCameraYCar) this.cameraRotationY = this.maxCameraYCar;
            if (this.cameraRotationY < -this.maxCameraYCar) this.cameraRotationY = -this.maxCameraYCar;
        }

        this.playerCamera.transform.localEulerAngles = new Vector3(this.cameraRotationX, this.cameraRotationY, 0);
    }
}
