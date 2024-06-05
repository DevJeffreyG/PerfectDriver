using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class keyBoardToggle : MonoBehaviour
{
    public Toggle tl;
    public void keyboard(){
        ProfileController.getProfile().getSettings().setSetting(Settings.SettingName.OnlyKeyboard, tl.isOn);
        int i = 0;
        GameObject[] menusOcultos = {null,null,null};

        // estoy cansado jefe

        foreach (Transform hijo in GameObject.Find("Canvas").transform)  // obtener menus
        {
            // Verifica si el hijo está desactivado
            if (!hijo.gameObject.activeSelf) {
                menusOcultos[i++] = hijo.gameObject; 
            }
        }
        Profile profile = ProfileController.getProfile();
        menusOcultos[0].SetActive(true);
        GameObject.Find("Canvas/PanelControles/PanelCamaraMov").SetActive((bool) profile.getSettings().getSetting(Settings.SettingName.OnlyKeyboard));
        menusOcultos[0].SetActive(false);
    }
}
