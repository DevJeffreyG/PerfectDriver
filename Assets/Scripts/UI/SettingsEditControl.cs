using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsEditControl : MonoBehaviour
{
    private KeyCode storedKey = KeyCode.None;
    public TMPro.TMP_Text txt;
    public String control;
    private bool init = false;
    void Start()
    {
        enabled = false;
    }
    void Update()
    {
        if (init){
            if (Input.anyKeyDown)
            {
                storedKey = GetPressedKey();
                enabled = false;
                init = false;
                txt.text = storedKey.ToString();
                Settings.SettingName r = (Settings.SettingName) Enum.Parse(typeof(Settings.SettingName), control);
                ProfileController.getProfile().getSettings().setSetting(r, storedKey);
                GameObject.Find("Canvas/PanelControles/PanelEspera").SetActive(false);
            } else {
                GameObject.Find("Canvas/PanelControles/PanelEspera").SetActive(true);
            }
        } else {
            init = true;
        }
    }

    private KeyCode GetPressedKey()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }
}
