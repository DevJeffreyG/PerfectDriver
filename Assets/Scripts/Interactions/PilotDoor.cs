using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDoor : Interactable
{
    public GameObject player;
    private PlayerController playerController;
    private GameObject car;
    private CarController carController;

    public override void Start()
    {
        base.Start(); // super()
        this.playerController = player.GetComponent<PlayerController>();
        car = this.transform.gameObject; // Est� en la puerta

        do // Sube hasta el padre (Car)
        {
            car = car.transform.parent.gameObject;
        } while (!car.CompareTag("UsableCar"));

        carController = car.GetComponent<CarController>();

    }

    public void interact()
    {
        this.onLostFocus();
        this.playerController.toggleInsideCar();

        this.playerController.setCar(car);
        
        GameObject seat = car.transform.Find("PilotSeat").gameObject;

        // Teletransportar al jugador dentro del carro
        this.player.transform.localScale = Vector3.one * 1.5f;
        this.player.transform.position = seat.transform.position;

        this.player.transform.SetParent(car.transform, true);
        carController.setPlayer(this.player);

        //this.player.GetComponent<PlayerController>().getCharacterController().enabled = true;
    }
}
