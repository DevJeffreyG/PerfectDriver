using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, Focusable
{
    private bool focused = false;
    private Outline outline;
    private Settings playerSettings;

    // https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/keywords/virtual
    public virtual void Start()
    {
        this.outline = this.AddComponent<Outline>();
        this.outline.OutlineWidth = 4f;
        this.outline.enabled = false;
        this.playerSettings = ProfileController.profile.getSettings();
    }

    void Update()
    {
        if(this.playerSettings.Down(Settings.SettingName.Interact) && this.focused)
        {
            this.SendMessage("interact");
        }
    }

    public void onFocus()
    {
        this.focused = true;
        this.outline.enabled = true;
    }

    public void onLostFocus()
    {
        this.outline.enabled = false;
        this.focused = false;
    }

    public abstract void interact();
}
