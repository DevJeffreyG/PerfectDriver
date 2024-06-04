using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private List<GameObject> active = new List<GameObject>();
    private GameObject obj;

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

                    active.Add(this.obj);

                    //Debug.Log("Viendo una puerta para entrar al auto");
                }
                else

                if (hit.collider.gameObject.CompareTag("CameraTrigger"))
                {
                    this.obj = Helper.FindChildByTag(hit.collider.gameObject.transform.parent, "ActiveOnLook");
                    if (this.obj != null)
                    {
                        this.obj.SendMessage("onFocus");
                        active.Add(this.obj);
                    }
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
        foreach (GameObject obj in this.active)
        {

            if (obj != null)
            {
                try
                {
                    obj.SendMessage("onLostFocus");
                    this.obj = null;
                }
                catch (Exception err)
                {
                    Debug.Log(err);
                }
            }
        }
    }
}
