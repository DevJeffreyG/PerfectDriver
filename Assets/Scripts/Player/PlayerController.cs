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

    private float defaultFOV;
    private float CurrentYSpeed = 0f;
    private float cameraRotationX = 0f;
    private float cameraRotationY = 0f;
    private bool isInCar = false;
    private GameObject car = null;

    private readonly float maxCameraYCar = 90f;
    private Settings playerSettings;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.detectCollisions = false;
        this.playerSettings = ProfileController.getProfile().getSettings();
        this.Sensibility = (float) this.playerSettings.getSetting(Settings.SettingName.CameraSens);

        this.defaultFOV = this.playerCamera.fieldOfView;

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
        // Si est� dentro de un carro el movimiento ser� distinto (el de un carro)
        if(this.isInCar) return;

        // Toma el sentido hacia donde est� viendo la c�mara del jugador
        Vector3 forward = this.playerCamera.transform.forward;
        Vector3 right = this.playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        // Con la norma del vector de la camara se puede mover en la direcci�n relativa a esta
        forward.Normalize();
        right.Normalize();

        int axisX = 0;
        if (this.playerSettings.Holding(Settings.SettingName.Right)) axisX = 1;
        if (this.playerSettings.Holding(Settings.SettingName.Left)) axisX = -1;

        int axisY = 0;
        if (this.playerSettings.Holding(Settings.SettingName.Accelerate)) axisY = 1;
        if (this.playerSettings.Holding(Settings.SettingName.Brake)) axisY = -1;

        Vector3 move = right * axisX + forward * axisY;

        // Si est� en el suelo
        if (this.characterController.isGrounded)
        {
            this.CurrentYSpeed = 0f;

            // Si est� saltando
            if (this.playerSettings.Down(Settings.SettingName.Jump))
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
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");

        if ((bool) this.playerSettings.getSetting(Settings.SettingName.OnlyKeyboard))
        {
            Debug.Log("Just Keyboard");
            xAxis = this.playerSettings.Holding(Settings.SettingName.RightCam) ? 1 : this.playerSettings.Holding(Settings.SettingName.LeftCam) ? - 1 : 0;
            yAxis = this.playerSettings.Holding(Settings.SettingName.UpCam) ? 1 : this.playerSettings.Holding(Settings.SettingName.DownCam) ? - 1 : 0;
        }

        if (this.playerSettings.Down(Settings.SettingName.Zoom))
        {
            StopAllCoroutines();
            StartCoroutine(changeFOV(this.playerCamera.fieldOfView, 15, 0.15f));
        } else if(this.playerSettings.Up(Settings.SettingName.Zoom))
        {
            StopAllCoroutines();
            StartCoroutine(changeFOV(this.playerCamera.fieldOfView, this.defaultFOV, 0.1f));
        }
        this.cameraRotationY += xAxis * this.Sensibility;
        this.cameraRotationX += yAxis * -1 * this.Sensibility;

        if (this.cameraRotationX > 90f) this.cameraRotationX = 90f;
        else if (this.cameraRotationX < -90f) this.cameraRotationX = -90f;

        // Si est� dentro de un carro, no puede girar tanto por estar sentado
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
        this.toggleEntranceLayer();
        this.toggleCharacterController();
    }

    private void toggleEntranceLayer()
    {
        GameObject entrance = Helper.FindChildByTag(GameObject.FindGameObjectWithTag("UsableCar").transform.Find("Body").transform, "CarEntrance");
        
        if (this.isInCar)
        {
            entrance.layer = 0; // DEFAULT
        } else
        {
            entrance.layer = 3; // INTERACTABLE LAYER
        }
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

    protected IEnumerator changeFOV(float from, float to, float delay)
    {
        float timeElapsed = 0f;

        while (timeElapsed < delay)
        {
            this.playerCamera.fieldOfView = Mathf.Lerp(from, to, timeElapsed / delay);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
