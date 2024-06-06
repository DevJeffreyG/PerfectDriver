using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Profile profile = ProfileController.getProfile();
        
        // Luces

        GameObject.Find("Canvas/PanelControles/PanelLuces/Button (5)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.DirectionalRight).ToString();
        GameObject.Find("Canvas/PanelControles/PanelLuces/Button (6)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.DirectionalLeft).ToString();
        GameObject.Find("Canvas/PanelControles/PanelLuces/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.EmergencyLights).ToString();
        GameObject.Find("Canvas/PanelControles/PanelLuces/Button (8)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.ToggleLights).ToString();

        // Movimiento

        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (1)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.Accelerate).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (2)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.Brake).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (3)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.Right).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (4)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.Left).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (5)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.GearUp).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (6)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.GearDown).ToString();
        GameObject.Find("Canvas/PanelControles/PanelMovimiento/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.StabilizeSteerWheel).ToString();        

        // Basico

        GameObject.Find("Canvas/PanelControles/PanelBasico/Button (5)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.ToggleEngine).ToString();
        GameObject.Find("Canvas/PanelControles/PanelBasico/Button (6)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.ToggleHandbrake).ToString();
        GameObject.Find("Canvas/PanelControles/PanelBasico/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.Interact).ToString();

        // vista
        
        GameObject.Find("Canvas/PanelControles/PanelVista/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.Zoom).ToString();
        GameObject.Find("Canvas/PanelControles/PanelVista/Button (8)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.Horn).ToString();
        GameObject.Find("Canvas/PanelControles/PanelVista").GetComponentInChildren<TMPro.TMP_InputField>().text = profile.getSettings().getSetting(Settings.SettingName.CameraSens).ToString();

        // camara

        GameObject.Find("Canvas/PanelControles/PanelCamaraMov/Button (5)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.UpCam).ToString();
        GameObject.Find("Canvas/PanelControles/PanelCamaraMov/Button (6)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.DownCam).ToString();
        GameObject.Find("Canvas/PanelControles/PanelCamaraMov/Button (7)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.LeftCam).ToString();
        GameObject.Find("Canvas/PanelControles/PanelCamaraMov/Button (8)").GetComponentInChildren<TMPro.TMP_Text>().text = profile.getSettings().getSetting(Settings.SettingName.RightCam).ToString();
        GameObject.Find("Canvas/PanelControles/PanelCamaraMov").SetActive((bool) profile.getSettings().getSetting(Settings.SettingName.OnlyKeyboard));
        
        // alcanse de vision 

        // menu graficos |||| Apartir de aqui no se puede acceder con Gameobject.Find

        int i = 0;
        GameObject[] menusOcultos = {null,null,null};
        foreach (Transform hijo in GameObject.Find("Canvas").transform)  // obtener menus
        {
            // Verifica si el hijo está desactivado
            if (!hijo.gameObject.activeSelf) {
                menusOcultos[i++] = hijo.gameObject; 
            }
        }
        menusOcultos[0].GetComponentInChildren<TMPro.TMP_InputField>().text = ""+ProfileController.getProfile().getSettings().getSetting(Settings.SettingName.MaxFPS);
        menusOcultos[2].GetComponentInChildren<Toggle>().isOn = (bool) profile.getSettings().getSetting(Settings.SettingName.OnlyKeyboard);
        menusOcultos[0].GetComponentInChildren<Slider>().value = (float) profile.getSettings().getSetting(Settings.SettingName.CameraDistance);
        //menusOcultos[0].GetComponentInChildren<Slider>().value = (float) profile.getSettings().getSetting(Settings.SettingName.CameraDistance);
        Slider[] sonidoSliders = menusOcultos[1].GetComponentsInChildren<Slider>();
        sonidoSliders[0].value = (float) profile.getSettings().getSetting(Settings.SettingName.BG_Volume);
        sonidoSliders[1].value = (float) profile.getSettings().getSetting(Settings.SettingName.SFX_Volume);
    }
}
