using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class KeyblindsParser : MonoBehaviour
{
    private TMP_Text Text;
   
    

    void Start()
    {
        string newChar;
        string identifier;
        Regex pattern = new Regex(@"\[(.*?)\]+");
        Text = GetComponent<TMP_Text>();

        MatchCollection collection = pattern.Matches(Text.text);

        foreach (Match match in collection)
        {
            GroupCollection groups = match.Groups;

            identifier = match.ToString().Replace("[", "").Replace("]", "");

            Settings.SettingName settingName = (Settings.SettingName)Enum.Parse(typeof(Settings.SettingName), identifier);
            Settings settings = ProfileController.getProfile().getSettings();

            KeyCode key = (KeyCode)settings.getSetting(settingName);

            newChar = pattern.Replace(Text.text, key.ToString(), 1);

            Text.text = newChar;
        }
        
    }

    
}
