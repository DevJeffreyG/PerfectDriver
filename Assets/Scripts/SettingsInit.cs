using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Luces

        GameObject.Find("Canvas/PanelControles/PanelLuces/Button (5)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.DirectionalRight).ToString();
        GameObject.Find("Canvas/PanelControles/PanelLuces/Button (6)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.DirectionalLeft).ToString();
        //GameObject.Find("Canvas/PanelControles/PanelLuces/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Emergencia).ToString();
        GameObject.Find("Canvas/PanelControles/PanelLuces/Button (8)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.ToggleLights).ToString();

        // Movimiento

        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (1)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Accelerate).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (2)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Brake).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (3)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Right).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (4)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Left).ToString();

        // Basico

        GameObject.Find("Canvas/PanelControles/PanelBasico/Button (5)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.ToggleEngine).ToString();
        GameObject.Find("Canvas/PanelControles/PanelBasico/Button (6)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.ToggleHandbrake).ToString();
        GameObject.Find("Canvas/PanelControles/PanelBasico/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Interact).ToString();
        GameObject.Find("Canvas/PanelControles/PanelBasico/Button (8)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Jump).ToString();

        //GameObject.Find("Canvas/PanelControles/PanelVista/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = ProfileController.profile.getSettings().getSetting(Settings.SettingName.Zoom).ToString();
    }
}
