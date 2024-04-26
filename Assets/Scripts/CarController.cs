using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float power = 2f;

    private GameObject player;
    private Rigidbody rb;
    private float speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        this.movementManager();
    }

    private void movementManager()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if (Mathf.Abs(this.speed) < 0.01f)
            {
                Debug.Log("Freno de mano completado");
                this.speed = 0;
                this.rb.velocity = Vector3.zero;
            } else
            {
                this.speed += this.speed > 0 ? Time.deltaTime * this.power * -1.5f : Time.deltaTime * this.power * 1.5f;
            }
        }
        else
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            if (inputY < 0 && this.speed > 0) inputY *= 3f;

            this.speed += this.power * inputY * Time.deltaTime;
            Vector3 forward = this.transform.forward.normalized;
            forward.y = 0;

            this.rb.AddForce(forward * speed, ForceMode.Acceleration);
        }

        Debug.Log(this.speed);
        Debug.DrawLine(this.transform.position, (this.transform.position + this.transform.forward.normalized * 5f * speed), Color.red);
    }
}
