using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDoor : Interactable
{
    private GameObject player;
    private PlayerController playerController;
    private GameObject car;
    private CarController carController;

    public override void Start()
    {
        base.Start(); // super()
        this.player = PlayerController.getPlayerObject();
        this.playerController = player.GetComponent<PlayerController>();
        car = this.transform.gameObject; // Está en la puerta

        do // Sube hasta el padre (Car)
        {
            car = car.transform.parent.gameObject;
        } while (!car.CompareTag("UsableCar"));

        carController = car.GetComponent<CarController>();

    }

    public override void interact()
    {
        this.onLostFocus();
        this.playerController.toggleInsideCar();

        this.playerController.setCar(car);
        
        GameObject seat = car.transform.Find("PilotSeat").gameObject;

        // Teletransportar al jugador dentro del carro
        this.player.transform.localScale = Vector3.one * 1.5f;
        this.player.transform.position = seat.transform.position;

        this.player.transform.SetParent(car.transform, true);
        this.player.transform.localRotation = Quaternion.identity; // Rotación 0,0,0

        carController.setPlayer(this.player);

        //this.player.GetComponent<PlayerController>().getCharacterController().enabled = true;
    }
}
