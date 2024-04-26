using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDoor : MonoBehaviour
{
    public GameObject player;

    public void interact()
    {
        this.player.GetComponent<PlayerController>().toggleInsideCar();
        GameObject seat = GameObject.Find("PilotSeat");
        GameObject car = GameObject.FindGameObjectWithTag("UsableCar");

        // Teletransportar al jugador dentro del carro
        this.player.transform.localScale = Vector3.one * 1.5f;
        this.player.transform.position = seat.transform.position;

        this.player.transform.SetParent(car.transform, true);
        car.AddComponent<CarController>();

        //this.player.GetComponent<PlayerController>().getCharacterController().enabled = true;
    }
}
