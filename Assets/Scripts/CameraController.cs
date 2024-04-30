using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public LayerMask mask;
    GameObject obj;

    // Update is called once per frame
    void Update()
    {

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, mask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.distance <= 4.5f)
            {
                if (hit.collider.gameObject.CompareTag("CarEntrance"))
                {
                    this.obj = hit.collider.gameObject;
                    this.obj.SendMessage("onFocus");

                    //Debug.Log("Viendo una puerta para entrar al auto");
                }
            } else
            {
                this.lostFocus();
            }
        } else
        {
            this.lostFocus();
        }
    }

    private void lostFocus()
    {
        if (this.obj != null)
        {
            try
            {
                this.obj.SendMessage("onLostFocus");
                this.obj = null;
            }
            catch (Exception err)
            {
                Debug.Log(err);
            }
        }
    }
}
