using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool interatable = false;
    private Outline outline;

    void Start()
    {
        this.outline = this.AddComponent<Outline>();
        this.outline.OutlineWidth = 4f;
        this.outline.enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && this.interatable)
        {
            this.SendMessage("interact");
            this.lostFocus();
        }
    }

    public void shine()
    {
        this.interatable = true;
        this.outline.enabled = true;
    }

    public void lostFocus()
    {
        Debug.Log("LOST FOCUS!");
        this.outline.enabled = false;
        this.interatable = false;
    }
}
