using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetFps : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_InputField textArea;
    public void Fps(){
        if (Int32.TryParse(textArea.text, out int r) && r > 0){
            ProfileController.getProfile().getSettings().setSetting(Settings.SettingName.MaxFPS, r);
        } else {
            textArea.text = "Solo numeros!";
        }
    }
}
