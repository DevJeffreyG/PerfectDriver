using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    private Settings settings;
    private int targetFPS = 120;
    private bool loadedSettingFPS = false;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = this.targetFPS;
    }

    void Update()
    {
        if (this.settings == null)
        {
            try
            {
                this.settings = ProfileController.getProfile().getSettings();

            }
            catch (NullReferenceException)
            {
                Debug.Log("Aun no se carga el perfil, usando 120FPS");
            }
        }

            if (this.settings != null && !this.loadedSettingFPS)
        {
            Debug.Log("FPS Settings cargados");
            loadedSettingFPS = true;
            Application.targetFrameRate = (int) this.settings.getSetting(Settings.SettingName.MaxFPS);
        }
    }
}
