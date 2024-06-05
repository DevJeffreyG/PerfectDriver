using System;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    private Settings settings;
    private float sfx, bg;

    [SerializeField] private AudioMixer SFXMixer, BGMixer;
    // Start is called before the first frame update
    void Start()
    {
        this.settings = ProfileController.getProfile().getSettings();
        this.updateVolume();
    }

    public void updateVolume()
    {
        this.sfx = (float) this.settings.getSetting(Settings.SettingName.SFX_Volume);
        this.bg = (float) this.settings.getSetting(Settings.SettingName.BG_Volume);

        this.SFXMixer.SetFloat("volume", (float) percToDb(this.sfx));
        this.BGMixer.SetFloat("volume", (float) percToDb(this.sfx));
    }

    private float percToDb(float perc)
    {
        float log = Mathf.Log10(perc);
        if (float.IsInfinity(log)) log = -4;

        return 20 * log;
    }
}
