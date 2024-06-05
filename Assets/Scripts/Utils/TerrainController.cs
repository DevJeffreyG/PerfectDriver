using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    private Settings settings;
    private Terrain Terrain;
    private GameObject houses;
    void Start()
    {
        houses = GameObject.FindGameObjectWithTag("Houses");
        settings = ProfileController.getProfile().getSettings();
        Terrain = GetComponent<Terrain>();
        updateTerrain();
    }

    public void updateTerrain()
    {
        if ((bool) settings.getSetting(Settings.SettingName.ShowTrees))
        {
            Terrain.treeLODBiasMultiplier = (float)settings.getSetting(Settings.SettingName.TreesRenderDistance);
        }
        else
        {
            Terrain.treeLODBiasMultiplier = 0;
        }

        if (!(bool) settings.getSetting(Settings.SettingName.ShowHouses) && houses != null) houses.SetActive(false);
    }
}
