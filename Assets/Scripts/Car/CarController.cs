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
        Reverse = 6
    }

    private bool justEntered = true;

    [SerializeField] private float maxMotorTorque = 4000f; // Potencia del motor, es ineficiente si se sube mucho sin cambiar propiedades de ruedas
    private float brakeTorque = 5000f; // Cantidad de freno, que tan bien podria frenar
    
    [SerializeField] private float maxSpeed = Conversor.KilometersPerHourToUnitsSecond(150); // 20 m/s, 72 km/h
    private float turnRange = 30f;
    private float turnRangeAtMaxSpeed = 10f;

    // SONIDOS
    private Transform sounds;
    LoopSound idleSound, drivingSound, turnSignalLoop, claxonLoop;
    SingleSound engineStart, engineAcceleration, turnSignalToggle, doorClosing, handbrakeUpSound, handbrakeDownSound;

    // FRENO DE MANO
    private bool isAnimatingHB = false;
    private float timeHB = 0f;

    // CAJA DE CAMBIOS
    private int motorGear = 1; // 0 reversa, 1, 2, 3, 4, 5
    private int badTransmissions = 0;
    private bool transmissionIsBroken = false; // Si la transmisi�n del carro est� da�ada

    // MANTENER EL ANGULOs
    private readonly float defaultSteerForce = 100f;
    private readonly float maxSteerAngle = 450f;
    private float actualSteerForce = 100f;
    private float hAngleMultiplier = 0f;
    private float steerWheelLocalTurn = 0f;
    private float idleCount = 0f;
    private bool isIdle = false;
    private bool idleAngle = false;

    private GameObject player = null;
    private GameObject steeringWheel = null;
    private PlayerController playerController = null;

    private Rigidbody carPhysics;
    private WheelControl[] wheels;
    private bool handbrakeIsUp = true;
    private bool engineStarted = false;

    // TABLERO
    private TMP_Text velocimetro, cambioActual;
    private GameObject agujaVelocimetro, turnSignalLeft, turnSignalRight;

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
    private bool EmergencyLights = false;
    private bool Reverse = false;

    private Material FLM;
    private Material TLM;
    private Material RLM;
    private Material ILM;
    private Material IRM;

    private readonly float indicatorsEvery = 0.382f;
    private float leftTimer;
    private float leftTimer2;
    private float rightTimer;
    private float rightTimer2;
    private float emergencyTimer;
    private float emergencyTimer2;

    private Settings playerSettings;

    void Start()
    {
        // Direccionales
        leftTimer = leftTimer2 = rightTimer = rightTimer2 = emergencyTimer = emergencyTimer2 = indicatorsEvery;
        
        // El componente de fisicas del carro
        carPhysics = GetComponent<Rigidbody>();
        
        // Cambios visuales
        steeringWheel = this.transform.Find("Body/SWheel").gameObject;
        agujaVelocimetro = this.transform.Find("Tablero/AgujaAxis").gameObject;
        velocimetro = this.transform.Find("Tablero/Speedometer").GetComponent<TMP_Text>();
        cambioActual = this.transform.Find("Tablero/Gear").GetComponent<TMP_Text>();
        turnSignalLeft = this.transform.Find("Tablero/TurnSignalLeft").gameObject;
        turnSignalRight = this.transform.Find("Tablero/TurnSignalRight").gameObject;

        turnSignalLeft.SetActive(false);
        turnSignalRight.SetActive(false);

        // Sonidos
        sounds = this.transform.Find("Sounds");

        idleSound = sounds.Find("IdleSound").GetComponent<LoopSound>();
        engineStart = sounds.Find("EngineStartSound").GetComponent<SingleSound>();
        doorClosing = sounds.Find("DoorClosing").GetComponent<SingleSound>();
        engineAcceleration = sounds.Find("EngineAcceleration").GetComponent<SingleSound>();
        engineAcceleration.setMaxVol(0.75f);
        handbrakeUpSound = sounds.Find("HandbrakeUp").GetComponent<SingleSound>();
        handbrakeDownSound = sounds.Find("HandbrakeDown").GetComponent<SingleSound>();

        drivingSound = sounds.Find("DrivingSound").GetComponent<LoopSound>();
        drivingSound.setMaxVol(0.5f);

        claxonLoop = sounds.Find("ClaxonLoop").GetComponent<LoopSound>();
        turnSignalToggle = sounds.Find("TurnSignalToggle").GetComponent<SingleSound>();
        turnSignalLoop = sounds.Find("TurnSignalLoop").GetComponent<LoopSound>();
        turnSignalLoop.setMaxVol(0.5f);

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

        this.playerSettings = ProfileController.getProfile().getSettings();
    }

    private void Update()
    {
        this.movementManager();
        this.keybindsManager();
        this.animationManager();
    }

    private void animationManager()
    {
        if (isAnimatingHB) animateHandbrake();

        // ACTUALIZACIONES DEL TABLERO
        // EN KILOMETROS POR HORA (3600s/1h)
        String styledSpeed = Math.Round(Conversor.UnitsSecondToKilometersPerHour(getSpeed())).ToString();

        velocimetro.text = styledSpeed;

        // MOSTRAR EL CAMBIO ACTUAL
        String gear = motorGear == 0 ? "R" : motorGear.ToString();
        cambioActual.text = gear;

        // DIRECCIONALES

        // SONIDOS
        if(this.DLeft || this.DRight || this.EmergencyLights)
        {
            turnSignalLoop.Play();
        } else
        {
            turnSignalLoop.Stop();
            turnSignalLoop.ResetAudio();
        }

        // LUCES & TABLERO
        if(this.EmergencyLights)
        {
            Debug.Log("PIGN!");
            if (emergencyTimer >= 0f)
            {
                this.manageLight(LightType.DirectionalLeft, true);
                turnSignalLeft.SetActive(true);
                this.manageLight(LightType.DirectionalRight, true);
                turnSignalRight.SetActive(true);

                emergencyTimer -= Time.deltaTime;
                emergencyTimer2 = indicatorsEvery;
            }
            if (emergencyTimer <= 0f)
            {
                this.manageLight(LightType.DirectionalLeft, false);
                turnSignalLeft.SetActive(false);
                this.manageLight(LightType.DirectionalRight, false);
                turnSignalRight.SetActive(false);

                emergencyTimer2 -= Time.deltaTime;
                if (emergencyTimer2 <= 0f) emergencyTimer = indicatorsEvery;
            }
        } else
        {
            this.manageLight(LightType.DirectionalLeft, false);
            turnSignalLeft.SetActive(false);
            this.manageLight(LightType.DirectionalRight, false);
            turnSignalRight.SetActive(false);
        }

        if (this.DLeft)
        {
            turnSignalRight.SetActive(false);
            if (leftTimer >= 0f)
            {
                this.manageLight(LightType.DirectionalLeft, true);
                turnSignalLeft.SetActive(true);

                leftTimer -= Time.deltaTime;
                leftTimer2 = indicatorsEvery;
            }
            if (leftTimer <= 0f)
            {
                this.manageLight(LightType.DirectionalLeft, false);
                turnSignalLeft.SetActive(false);

                leftTimer2 -= Time.deltaTime;
                if (leftTimer2 <= 0f) leftTimer = indicatorsEvery;
            }
        }
        else if(!this.EmergencyLights)
        {
            this.manageLight(LightType.DirectionalLeft, false);
            turnSignalLeft.SetActive(false);
        }

        if (this.DRight)
        {
            turnSignalLeft.SetActive(false);
            if (rightTimer >= 0f)
            {
                this.manageLight(LightType.DirectionalRight, true);
                turnSignalRight.SetActive(true);
                
                rightTimer -= Time.deltaTime;
                rightTimer2 = indicatorsEvery;
            }
            if (rightTimer <= 0f)
            {
                this.manageLight(LightType.DirectionalRight, false);
                turnSignalRight.SetActive(false);
                
                rightTimer2 -= Time.deltaTime;
                if (rightTimer2 <= 0f) rightTimer = indicatorsEvery;
            }
        }
        else if(!this.EmergencyLights)
        {
            this.manageLight(LightType.DirectionalRight, false);
            turnSignalRight.SetActive(false);
        }
    }

    private void keybindsManager()
    {
        if (this.player == null) return;
        if(this.justEntered && this.playerSettings.Up(Settings.SettingName.Interact))
        {
            doorClosing.Play();
            this.justEntered = false;
            return;
        }

        if(this.playerSettings.Down(Settings.SettingName.ToggleEngine))
            this.toggleEngine();

        if(this.playerSettings.Down(Settings.SettingName.Accelerate) && this.engineStarted)
        {
            this.engineAcceleration.ResetAudio();
            this.engineAcceleration.Play(0.1f, 2);
        }

        if(this.playerSettings.Up(Settings.SettingName.Accelerate))
        {
            this.engineAcceleration.Stop(0.1f, true);
        }

        if (this.playerSettings.Down(Settings.SettingName.ToggleHandbrake))
        {
            this.handbrakeIsUp = !this.handbrakeIsUp;

            if(handbrakeIsUp)
            {
                handbrakeUpSound.ResetAudio();
                handbrakeUpSound.Play();
            } else
            {
                handbrakeDownSound.ResetAudio();
                handbrakeDownSound.Play();
            }

            timeHB = 0f;
            isAnimatingHB = true;
        }
        
        if(this.playerSettings.Down(Settings.SettingName.Interact) && !this.justEntered) {
            this.player = null;
            this.justEntered = true; // Reiniciar al estado original, para luego evitar bugs con el intermitente derecho
            this.playerController.getOutOfCar();
            this.playerController = null;
        }

        if (this.playerSettings.Down(Settings.SettingName.ToggleLights))
        {
            this.manageLight(LightType.Headlight, !this.Headlight);
        }

        if (this.playerSettings.Down(Settings.SettingName.DirectionalRight) && !this.justEntered && !this.EmergencyLights)
        {
            if (this.DLeft)
            { // Si el direccional izquierdo esta encendido
                this.DLeft = !this.DLeft;
                this.manageLight(LightType.DirectionalLeft, this.DLeft);
            }

            this.DRight = !this.DRight;
            this.manageLight(LightType.DirectionalRight, this.DRight);
            this.turnSignalToggle.Play();
        }

        if (this.playerSettings.Down(Settings.SettingName.DirectionalLeft) && !this.EmergencyLights)
        {
            if (this.DRight)
            { // Si el direccional derecho esta encendido
                this.DRight = !this.DRight;
                this.manageLight(LightType.DirectionalRight, this.DRight);
            }

            this.DLeft = !this.DLeft;
            this.manageLight(LightType.DirectionalLeft, this.DLeft);
        }

        if(this.playerSettings.Down(Settings.SettingName.EmergencyLights))
        {
            this.EmergencyLights = !this.EmergencyLights;

            if(this.EmergencyLights)
            {
                this.DRight = false;
                this.DLeft = false;
            }
            this.turnSignalToggle.Play();
        }

        if (this.playerSettings.Down(Settings.SettingName.Horn))
        {
            this.claxonLoop.Play(.02f);
        } else

        if(this.playerSettings.Up(Settings.SettingName.Horn))
        {
            this.claxonLoop.Stop(.1f, true);
        }

        if (this.playerSettings.Holding(Settings.SettingName.StabilizeSteerWheel))
        {
            this.centerSteerWheel();
        } else // Para que no sea posible darle los dos al tiempo
        {
            if (this.playerSettings.Holding(Settings.SettingName.FasterSteering))
            {
                this.increaseSteerForce();
            }
            else
            {
                this.actualSteerForce = this.defaultSteerForce;
            }
        }

        if(this.playerSettings.Down(Settings.SettingName.GearDown))
        {
            this.motorGear--;

            if(this.motorGear == 0 && Conversor.UnitsSecondToKilometersPerHour(getSpeed()) > 5f && this.engineStarted)
            {
                badTransmissions++;
            }
        } else if(this.playerSettings.Down(Settings.SettingName.GearUp))
        {
            this.motorGear++;

            if(this.motorGear == 1 && Conversor.UnitsSecondToKilometersPerHour(getSpeed()) > 5f && this.engineStarted)
            {
                badTransmissions++;
            }
        }

        if(this.motorGear < 0) this.motorGear = 0;
        if (this.motorGear > 5) this.motorGear = 5;

        this.manageLight(LightType.Reverse, this.motorGear == 0);

        if (badTransmissions >= 5)
        {
            transmissionIsBroken = true;
        }
    }

    private void movementManager()
    {        
        float vInput = this.playerSettings.Holding(Settings.SettingName.Accelerate) ? 1 : this.playerSettings.Holding(Settings.SettingName.Brake) ? -1 : 0;
        float hInput = this.playerSettings.Holding(Settings.SettingName.Right) ? 1 : this.playerSettings.Holding(Settings.SettingName.Left) ? -1 : 0;
        float speed = this.getSpeed();

        if (this.player == null) return;
        if(getSpeed() > 1)
        {
            this.drivingSound.Play(5f);
        } else
        {
            this.drivingSound.Stop(0.25f, true);
        }

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

        this.steerWheelLocalTurn += hInput * Time.deltaTime * this.actualSteerForce;
        if (Mathf.Abs(this.steerWheelLocalTurn) > maxSteerAngle)
        {
            this.actualSteerForce = this.defaultSteerForce;
            steerWheelLocalTurn = Math.Sign(this.steerWheelLocalTurn) * maxSteerAngle;
        }

            if (isIdle && !idleAngle)
        {
            this.idleAngle = this.centerSteerWheel();
        }

        this.hAngleMultiplier = Mathf.Lerp(0, Mathf.Sign(this.steerWheelLocalTurn), Mathf.Sign(this.steerWheelLocalTurn) * this.steerWheelLocalTurn / maxSteerAngle);              

        // Calcula la velocidad actual
        float forwardSpeed = Vector3.Dot(transform.forward, carPhysics.velocity);

        //Debug.Log("Current speed: " + forwardSpeed);

        // Calcula que tan cerca est� el carro de llegar a su velocidad m�xima (del cambio actual)
        // Con un numero de 0 a 1
        float currentMaxSpeed = maxSpeed * motorGear / 5;
        if (currentMaxSpeed == 0) currentMaxSpeed = maxSpeed * 1 / 5;

        float maxSpeedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        float speedFactor = Mathf.InverseLerp(0, currentMaxSpeed, forwardSpeed);

        // Cambiar la aguja del velocimetro
        agujaVelocimetro.transform.localEulerAngles = new Vector3(-90 + 180f * maxSpeedFactor, 0, 0);

        // Con ese numero se obtiene cuanto torque est� disponible
        // Si speedFactor est� en 1 (MAX VELOCIDAD) currentMotorTorque es 0, y viceversa
        float currentMotorTorque = Mathf.Lerp(maxMotorTorque, 0, speedFactor);

        // Calcular qu� tanto puede girar
        // Porque el carro gira m�s a m�xima velocidad
        float currentSteerRange = Mathf.Lerp(turnRange, turnRangeAtMaxSpeed, speedFactor);

        // Revisar si el jugador est� presionando el boton de acelerar
        bool isAccelerating = Mathf.Sign(vInput) == 1 && !transmissionIsBroken;// && vInput != 0;

        // Mover el volante
        steeringWheel.transform.localEulerAngles = new Vector3(steeringWheel.transform.localEulerAngles.x, steeringWheel.transform.localEulerAngles.y, steerWheelLocalTurn);

        foreach (WheelControl wheel in wheels)
        {
            //Debug.Log("MOTORTORQUEAVAILABLE: " + currentMotorTorque);
            //Debug.Log("MAXSPEEDAVAILABLE: " + Conversor.UnitsSecondToKilometersPerHour(currentMaxSpeed));

            if(wheel.canTurn())
            {
                wheel.setTurnAngle(hAngleMultiplier * currentSteerRange);
            }

            if (vInput == 0) // El jugador no est� ni acelerando ni desacelerando
            {
                // Si no est� cambiando de posicion el carro y est� encendido
                this.manageLight(LightType.Brake, this.engineStarted && speed == 0);
            }

            if (handbrakeIsUp) // Est� el freno de mano puesto
            {
                this.brake(wheel, 1f);
            } else if (isAccelerating) // El usuario est� yendo a alguna direccion, y esta es la que ya tenia el carro
            {
                // Detectar que esté en primera al arrancar, o en reversa
                if (speed == 0 && this.motorGear > 1 && vInput == 1 && this.engineStarted) this.toggleEngine();

                if (!this.engineStarted) // Si no ha iniciado el carro no puede ACELERAR, FRENAR SI
                {
                    vInput = 0f;
                }

                this.manageLight(LightType.Reverse, vInput < 0); // Y es hacia atr�s?
                this.manageLight(LightType.Brake, false);

                if (wheel.canAccelerate())
                {
                    int direction = this.motorGear == 0 ? -1 : 1; // Si est� en reversa, el torque ser� negativo
                    wheel.accelerate(vInput * currentMotorTorque * direction);
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
        this.increaseSteerForce();
        this.steerWheelLocalTurn += -1 * Mathf.Sign(this.steerWheelLocalTurn) * Time.deltaTime * actualSteerForce;

        if (Mathf.Abs(this.steerWheelLocalTurn) <= 1.5f)
        {
            this.steerWheelLocalTurn = 0f;
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

    private float getSpeed()
    {
        float speed = Mathf.Abs(this.carPhysics.velocity.magnitude);
        if (speed < 0.009f) speed = 0f;
        return speed;
    }

    public float getKmH()
    {
        return Conversor.UnitsSecondToKilometersPerHour(this.getSpeed());
    }

    private void setEngineStarted(bool engine)
    {
        this.engineStarted = engine;
    }

    private void toggleEngine()
    {
        this.engineStarted = !this.engineStarted;
        Debug.Log(this.engineStarted);

        if (this.engineStarted)
        {
            this.engineStart.Play(0, 7f);
            this.idleSound.Play(3f);
        }
        else
        {
            this.engineStart.Stop();
            this.idleSound.Stop(0.15f, true);
        }
    }

    private void increaseSteerForce()
    {
        this.actualSteerForce += this.defaultSteerForce * Time.deltaTime;
    }

    private void animateHandbrake()
    {
        GameObject handbrake = this.transform.Find("Body/HandBrake").gameObject;
        Vector3 objetive = this.handbrakeIsUp ? Vector3.zero : new Vector3(30, 0, 0);

        //Debug.Log(handbrake.transform.localEulerAngles);

        handbrake.transform.localEulerAngles = Vector3.Lerp(handbrake.transform.localEulerAngles, objetive, timeHB);
        timeHB += Time.deltaTime;

        if(timeHB >= 1)
        {
            isAnimatingHB = false;
        }
    }
}
