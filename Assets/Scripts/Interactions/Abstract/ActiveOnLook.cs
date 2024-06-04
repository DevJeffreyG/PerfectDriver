using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveOnLook : MonoBehaviour, Focusable
{
    public virtual void Start()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void onFocus()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void onLostFocus()
    {
        this.gameObject.SetActive(false);
    }
}
