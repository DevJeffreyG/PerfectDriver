using System;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private enum LightType
    {
        Brake =  1,
        Headlight = 2,
        FullBeam = 3,
        DirectionalRight = 4,
        DirectionalLeft = 5,
        Reverse = 6,
    }

    private bool justEntered = true;

    private float motorTorque = 2000f; // Potencia del motor, es ineficiente si se sube mucho sin cambiar propiedades de ruedas
    private float brakeTorque = 2000f; // Cantidad de freno, que tan bien podria frenar
    
    private float maxSpeed = Conversor.MetersToUnits(20); // 20 m/s, 72 km/h
    private float turnRange = 30f;
    private float turnRangeAtMaxSpeed = 10f;

    // MANTENER EL ANGULO
    private float steerForce = 5f;
    private float hHistory = 0f;
    private float idleCount = 0f;
    private bool isIdle = false;
    private bool idleAngle = false;

    private GameObject player = null;
    private GameObject steeringWheel = null;
    private PlayerController playerController = null;

    private Rigidbody carPhysics;
    private WheelControl[] wheels;
    private bool handBrake = true;
    private bool engineStarted = false;

    // MOSTRAR VELOCIDAD
    private TMP_Text velocimetro;
    private GameObject agujaVelocimetro;

    // LUCES
    private GameObject frontLights;
    private GameObject tailLights;
    private GameObject brakeLights;
    private GameObject reverseLights;
    private GameObject leftIndLights;
    private GameObject rightIndLights;

    private bool Headlight = false;
    private bool Brake = false;
    private bool DLeft = false;
    private bool DRight = false;
    private bool Reverse = false;

    private Material FLM;
    private Material TLM;
    private Material RLM;
    private Material ILM;
    private Material IRM;

    private readonly float indicatorsEvery = 0.5f;
    private float leftTimer;
    private float leftTimer2;
    private float rightTimer;
    private float rightTimer2;

    void Start()
    {
        leftTimer = leftTimer2 = rightTimer = rightTimer2 = indicatorsEvery;
        carPhysics = GetComponent<Rigidbody>();
        steeringWheel = this.transform.Find("Body/SWheel").gameObject;
        agujaVelocimetro = this.transform.Find("Tablero/AgujaAxis").gameObject;
        velocimetro = this.transform.Find("Tablero/Speedometer").GetComponent<TMP_Text>();

        // Ajusta el centro de masa del carro para evitar que pasen cosas raras
        carPhysics.centerOfMass += Vector3.up * -1f;

        // Busca todas las ruedas que tenga el carro con el componente de WheelControl
        wheels = GetComponentsInChildren<WheelControl>();

        // LUCES
        FLM = Resources.Load<Material>("Imports/SportCar/Materials/Lights/FrontLights");
        TLM = Resources.Load<Material>("Imports/SportCar/Materials/Lights/BrakeLights");
        RLM = Resources.Load<Material>("Imports/SportCar/Materials/Lights/ReverseLight");
        ILM = Resources.Load<Material>("Imports/SportCar/Materials/Lights/LeftIndicator");
        IRM = Resources.Load<Material>("Imports/SportCar/Materials/Lights/RightIndicator");

        frontLights = this.transform.Find("Lights/FrontLights").gameObject;
        tailLights = this.transform.Find("Lights/TailLights").gameObject;
        brakeLights = this.transform.Find("Lights/BrakeLights").gameObject;
        reverseLights = this.transform.Find("Lights/ReverseLights").gameObject;
        leftIndLights = this.transform.Find("Lights/LeftIndicators").gameObject;
        rightIndLights = this.transform.Find("Lights/RightIndicators").gameObject;

        this.manageLight(LightType.Headlight, Headlight);
        this.manageLight(LightType.Brake, Brake);
        this.manageLight(LightType.DirectionalLeft, DLeft);
        this.manageLight(LightType.DirectionalRight, DRight);
        this.manageLight(LightType.Reverse, Reverse);

        FLM.DisableKeyword("_EMISSION");
        TLM.DisableKeyword("_EMISSION");
        RLM.DisableKeyword("_EMISSION");
        ILM.DisableKeyword("_EMISSION");
        IRM.DisableKeyword("_EMISSION");
    }

    private void Update()
    {
        this.movementManager();
        this.keybindsManager();
        this.indicatorsManager();
    }

    private void keybindsManager()
    {
        if (this.player == null) return;
        if(this.justEntered && Input.GetKeyUp(KeyCode.E))
        {
            this.justEntered = false;
            return;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            this.engineStarted = !this.engineStarted;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.handBrake = !this.handBrake;
        }
        
        if(Input.GetKeyDown(KeyCode.F)) {
            this.player = null;
            this.justEntered = true; // Reiniciar al estado original, para luego evitar bugs con el intermitente derecho
            this.playerController.getOutOfCar();
            this.playerController = null;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            this.manageLight(LightType.Headlight, !this.Headlight);
        }

        if (Input.GetKeyDown(KeyCode.E) && !this.justEntered)
        {
            if (this.DLeft)
            { // Si el direccional izquierdo está encendido
                this.DLeft = !this.DLeft;
                this.manageLight(LightType.DirectionalLeft, this.DLeft);
            }

            this.DRight = !this.DRight;
            this.manageLight(LightType.DirectionalRight, this.DRight);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (this.DRight)
            { // Si el direccional derecho está encendido
                this.DRight = !this.DRight;
                this.manageLight(LightType.DirectionalRight, this.DRight);
            }

            this.DLeft = !this.DLeft;
            this.manageLight(LightType.DirectionalLeft, this.DLeft);
        }

        if(Input.GetKey(KeyCode.CapsLock))
        {
            this.centerSteerWheel();
        }
    }

    private void movementManager()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        float speed = Mathf.Abs(carPhysics.velocity.magnitude);
        if (speed < 0.0009f) speed = 0f;

        // EN KILOMETROS POR HORA (3600s/1h)
        String styledSpeed = Math.Round(Conversor.UnitsToKilometers(speed) * 3600, 1).ToString();

        if (this.player == null) return;
        velocimetro.text = styledSpeed;

        Debug.Log(idleCount);

        // Detectar actividad en el volante
        if (!isIdle && hInput == 0 && speed > 0.1f)
        {
            idleCount += Time.deltaTime;

            if(idleCount > 2f)
            {
                isIdle = true;
            }
        } else if(hInput != 0)
        {
            idleAngle = false;
            isIdle = false;
            idleCount = 0f;
        }

        this.hHistory += hInput * Time.deltaTime * this.steerForce;

        if (this.hHistory > 1f) hHistory = 1f;
        else if (this.hHistory < -1f) hHistory = -1f;
        
        if(isIdle && !idleAngle)
        {
            this.idleAngle = this.centerSteerWheel();
        }

        // Calcula la velocidad actual
        float forwardSpeed = Vector3.Dot(transform.forward, carPhysics.velocity);

        //Debug.Log("Current speed: " + forwardSpeed);

        // Calcula que tan cerca está el carro de llegar a su velocidad máxima
        // Con un numero de 0 a 1
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Cambiar la aguja del velocimetro
        agujaVelocimetro.transform.localEulerAngles = new Vector3(-90 + 180f * speedFactor, 0, 0);

        // Con ese numero se obtiene cuanto torque está disponible
        // Si speedFactor está en 1 (MAX VELOCIDAD) currentMotorTorque es 0, y viceversa
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // Calcular qué tanto puede girar
        // Porque el carro gira más a máxima velocidad
        float currentSteerRange = Mathf.Lerp(turnRange, turnRangeAtMaxSpeed, speedFactor);

        // Revisar si el jugador está yendo a la misma dirección que el carro en ese instante
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        // Mover el volante
        steeringWheel.transform.localEulerAngles = new Vector3(steeringWheel.transform.localEulerAngles.x, steeringWheel.transform.localEulerAngles.y, hHistory * currentSteerRange);

        foreach (WheelControl wheel in wheels)
        {
            if(wheel.canTurn())
            {
                wheel.setTurnAngle(hHistory * currentSteerRange);
            }

            if (handBrake) // Está el freno de mano puesto
            {
                this.brake(wheel, 1f);

                if(this.carPhysics.velocity.magnitude < 0.1f)
                    this.manageLight(LightType.Reverse, false);
            }
            else if (vInput == 0) // El jugador no está ni acelerando ni desacelerando
            {
                // Si no está cambiando de posicion el carro y está encendido
                this.manageLight(LightType.Brake, this.engineStarted && this.carPhysics.velocity.magnitude < 0.1f);
                
            } else if (isAccelerating) // El usuario está yendo a alguna direccion, y esta es la que ya tenia el carro
            {
                if (!this.engineStarted) // Si no ha iniciado el carro no puede ACELERAR, FRENAR SI
                {
                    vInput = 0f;
                }

                this.manageLight(LightType.Reverse, vInput < 0); // Y es hacia atrás?
                this.manageLight(LightType.Brake, false);

                if (wheel.canAccelerate())
                {
                    wheel.accelerate(vInput * currentMotorTorque);
                }

                wheel.setBrakeTorque(0);
            } else
            {
                // El jugador quiere ir en contra de la direccion actual
                // (Frenar)
                this.brake(wheel, vInput);
            }
        }

        Debug.DrawLine(this.transform.position + new Vector3(0, 5f, 0), this.transform.position + new Vector3(0, 5f, 0) + this.transform.forward.normalized * forwardSpeed, Color.green);
    }

    private void indicatorsManager()
    {
        if (this.DLeft)
        {
            if (leftTimer >= 0f)
            {
                leftTimer -= Time.deltaTime;
                this.manageLight(LightType.DirectionalLeft, true);
                leftTimer2 = indicatorsEvery;
            }
            if (leftTimer <= 0f)
            {
                this.manageLight(LightType.DirectionalLeft, false);
                leftTimer2 -= Time.deltaTime;
                if (leftTimer2 <= 0f) leftTimer = indicatorsEvery;
            }
        }

        if (this.DRight)
        {
            if (rightTimer >= 0f)
            {
                rightTimer -= Time.deltaTime;
                this.manageLight(LightType.DirectionalRight, true);
                rightTimer2 = indicatorsEvery;
            }
            if (rightTimer <= 0f)
            {
                this.manageLight(LightType.DirectionalRight, false);
                rightTimer2 -= Time.deltaTime;
                if (rightTimer2 <= 0f) rightTimer = indicatorsEvery;
            }
        }
        else
        {
            this.manageLight(LightType.DirectionalRight, false);
        }
    }
    private void brake(WheelControl wheel, float input)
    {
        this.manageLight(LightType.Brake, true);

        wheel.setBrakeTorque(Mathf.Abs(input) * brakeTorque);
        wheel.setMotorTorque(0);
    }

    private void manageLight(LightType type, bool on)
    {
        switch (type)
        {
            case LightType.Headlight:
                frontLights.SetActive(on);
                tailLights.SetActive(on);
                
                this.Headlight = on;

                if(on)
                {
                    this.FLM.EnableKeyword("_EMISSION");
                } else
                {
                    this.FLM.DisableKeyword("_EMISSION");
                }
                break;

            case LightType.Brake:
                brakeLights.SetActive(on);

                this.Brake = on;

                if (on)
                {
                    this.TLM.EnableKeyword("_EMISSION");
                }
                else
                {
                    this.TLM.DisableKeyword("_EMISSION");
                }
                break;

            case LightType.DirectionalLeft:
                leftIndLights.SetActive(on);

                if (on)
                {
                    this.ILM.EnableKeyword("_EMISSION");
                }
                else
                {
                    this.ILM.DisableKeyword("_EMISSION");
                }
                break;

            case LightType.DirectionalRight:
                rightIndLights.SetActive(on);

                if (on)
                {
                    this.IRM.EnableKeyword("_EMISSION");
                }
                else
                {
                    this.IRM.DisableKeyword("_EMISSION");
                }
                break;

            case LightType.Reverse:
                reverseLights.SetActive(on);

                this.Reverse = on;

                if (on)
                {
                    this.RLM.EnableKeyword("_EMISSION");
                }
                else
                {
                    this.RLM.DisableKeyword("_EMISSION");
                }
                break;
        }
    }

    private bool centerSteerWheel()
    {
        this.hHistory += -1 * Mathf.Sign(this.hHistory) * Time.deltaTime;

        if (Mathf.Abs(this.hHistory) <= 0.0009)
        {
            this.hHistory = 0f;
            return true;
        } else
        {
            return false;
        }
    }
    public void setPlayer(GameObject player)
    {
        this.player = player;
        this.playerController = this.player.GetComponent<PlayerController>();
    }
}
