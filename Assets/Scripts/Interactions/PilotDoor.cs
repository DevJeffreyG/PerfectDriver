using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDoor : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;

    private void Start()
    {
        this.playerController = player.GetComponent<PlayerController>();
    }

    public void interact()
    {
        this.playerController.toggleInsideCar();
        GameObject car = this.transform.gameObject;
        CarController carController;

        do
        {
            car = car.transform.parent.gameObject;
        } while (!car.CompareTag("UsableCar"));

        carController = car.GetComponent<CarController>();
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
