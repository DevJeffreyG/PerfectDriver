using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public Camera playerCamera;
    private GameObject pilotDoor = null;

    public float MovementSpeed = 12f;
    public float JumpSpeed = 3f;
    public float Sensibility = 5f;

    private float CurrentYSpeed = 0f;
    private float cameraRotationX = 0f;
    private float cameraRotationY = 0f;
    private bool isInCar = false;
    private GameObject car = null;

    private readonly float maxCameraYCar = 40f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        this.cameraManager();
        this.movementManager();
    }

    public static GameObject getPlayerObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
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
            this.CurrentYSpeed = 0f;

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

    public GameObject getCameraObject()
    {
        return this.playerCamera.gameObject;
    }

    public void toggleInsideCar()
    {
        this.isInCar = !this.isInCar;
        this.toggleRaycast();
        this.toggleCharacterController();
    }

    private void toggleRaycast()
    {
        CameraController raycast = this.GetComponentInChildren<CameraController>();

        raycast.enabled = !raycast.enabled;
    }

    private void toggleCharacterController()
    {
        this.characterController.enabled = !this.characterController.enabled;
    }

    public void setCar(GameObject car)
    {
        this.car = car;

        foreach(Transform child in this.car.transform.Find("Body").transform)
        {
            if(child.CompareTag("CarEntrance"))
            {
                this.pilotDoor = child.gameObject;
            }
        }
    }

    public void getOutOfCar()
    {
        Vector3 v = pilotDoor.transform.position + (pilotDoor.transform.right * -1).normalized * 7f; // La posicion a un lado de la puerta del piloto

        this.transform.position = v;
        this.transform.localScale = Vector3.one * 3f;

        this.transform.SetParent(null, true);
        this.car = null;
        this.pilotDoor = null;

        this.toggleInsideCar(); // Volver a darle el control al jugador
    }
}
