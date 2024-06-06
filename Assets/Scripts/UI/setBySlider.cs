using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class setBySlider : MonoBehaviour
{
    public Slider scb;
    public void setProperty(string property){
        Settings.SettingName r = (Settings.SettingName) Enum.Parse(typeof(Settings.SettingName), property);
        ProfileController.getProfile().getSettings().setSetting(r, scb.value);
    }
    
}
