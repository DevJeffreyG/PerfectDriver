using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class setCamSensitivity : MonoBehaviour
{
    public TMPro.TMP_InputField textArea;
    public void Sensitivity(){
        if (float.TryParse(textArea.text, out float r) && r > 0){
            ProfileController.getProfile().getSettings().setSetting(Settings.SettingName.CameraSens, r);
        } else {
            textArea.text = "Solo numeros!";
        }
    }
}
