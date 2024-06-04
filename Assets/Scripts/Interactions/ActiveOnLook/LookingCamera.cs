using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingCamera : ActiveOnLook
{
    private Camera Camera;
    public override void Start()
    {
        this.Camera = this.gameObject.GetComponent<Camera>();
        this.Camera.enabled = false;
    }
    public override void onFocus()
    {
        this.Camera.enabled = true;
    }

    public override void onLostFocus()
    {
        this.Camera.enabled = false;
    }
}
