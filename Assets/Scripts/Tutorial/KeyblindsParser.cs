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
        //  MatchCollection collection = pattern.Matches(Text.text).ToString().Replace("[", "").Replace("]", "");

        foreach (Match match in collection)
        {
            GroupCollection groups = match.Groups;
            Debug.Log(groups["word"].Value);
            Debug.Log(groups[0]);
            Debug.Log(groups[1]);
        }
        Settings.SettingName settingName = (Settings.SettingName)Enum.Parse(typeof(Settings.SettingName), "Right");
        Settings settings = ProfileController.getProfile().getSettings();

        KeyCode key = (KeyCode)settings.getSetting(settingName);

        newChar = Regex.Replace(Text.text, @"\[(.*?)\]", key.ToString());

        Text.text = newChar;
        Debug.Log(key);
    }

    
}
