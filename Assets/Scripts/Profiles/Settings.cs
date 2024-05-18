
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Settings
{
    // DEFAULTS
    public static readonly KeyCode D_ACCELERATE = KeyCode.W;
    public static readonly KeyCode D_LEFT = KeyCode.A;
    public static readonly KeyCode D_BRAKE = KeyCode.S;
    public static readonly KeyCode D_RIGHT = KeyCode.D;
    public static readonly KeyCode D_JUMP = KeyCode.Space;
    public static readonly KeyCode D_INTERACT = KeyCode.F;
    
    public static readonly KeyCode D_DIRECTIONAL_RIGHT = KeyCode.E;
    public static readonly KeyCode D_DIRECTIONAL_LEFT = KeyCode.Q;
    public static readonly KeyCode D_TOGGLE_ENGINE = KeyCode.R;
    public static readonly KeyCode D_TOGGLE_HANDBRAKE = KeyCode.Space;
    public static readonly KeyCode D_TOGGLE_LIGHTS = KeyCode.C;
    public static readonly KeyCode D_GEAR_DOWN = KeyCode.Alpha1;
    public static readonly KeyCode D_GEAR_UP = KeyCode.Alpha2;
    public static readonly KeyCode D_STABILIZE_STEER_WHEEL = KeyCode.CapsLock;

    public static readonly float D_CAMERA_SENS = 5f;
    public static readonly int D_MAX_FPS = 120;

    private FileInfo file;

    public enum SettingName
    {
        Accelerate, Left, Brake, Right, Jump, Interact, DirectionalRight, DirectionalLeft, ToggleEngine, ToggleHandbrake, ToggleLights, CameraSens, MaxFPS,
        GearUp, GearDown, StabilizeSteerWheel
    }

    private Dictionary<SettingName, object> defaultSettings, settingsMap;

    public Settings(FileInfo file)
    {
        this.createMap(ref defaultSettings);
        this.createMap(ref settingsMap);
        this.file = file;
        this.readFile();
    }

    // Es pasado como referencia, para cambiar directamente el mapa que se pase dentro de la instancia
    private void createMap(ref Dictionary<SettingName, object> map)
    {
        if(map == null)
        {
            map = new Dictionary<SettingName, object>();
        }

        map.Add(SettingName.Accelerate, D_ACCELERATE);
        map.Add(SettingName.Left, D_LEFT);
        map.Add(SettingName.Brake, D_BRAKE);
        map.Add(SettingName.Right, D_RIGHT);
        map.Add(SettingName.Jump, D_JUMP);
        map.Add(SettingName.Interact, D_INTERACT);

        map.Add(SettingName.DirectionalRight, D_DIRECTIONAL_RIGHT);
        map.Add(SettingName.DirectionalLeft, D_DIRECTIONAL_LEFT);
        map.Add(SettingName.ToggleEngine, D_TOGGLE_ENGINE);
        map.Add(SettingName.ToggleHandbrake, D_TOGGLE_HANDBRAKE);
        map.Add(SettingName.ToggleLights, D_TOGGLE_LIGHTS);
        map.Add(SettingName.GearDown, D_GEAR_DOWN);
        map.Add(SettingName.GearUp, D_GEAR_UP);
        map.Add(SettingName.StabilizeSteerWheel, D_STABILIZE_STEER_WHEEL);

        map.Add(SettingName.CameraSens, D_CAMERA_SENS);
        map.Add(SettingName.MaxFPS, D_MAX_FPS);
    }

    private void readFile()
    {
        StreamReader reader = new StreamReader(this.file.FullName);

        if(File.ReadLines(this.file.FullName).Count() != defaultSettings.Count)
        {
            reader.Close();

            this.setDefaults();
            this.saveFile();
        } else
        {
            try
            {
                foreach (var entry in settingsMap.ToList())
                {
                    Type type = entry.Value.GetType();

                    if (type == typeof(KeyCode))
                    {
                        settingsMap[entry.Key] = parseKeyCode(reader);
                    }
                    else if (type == typeof(int))
                    {
                        settingsMap[entry.Key] = parseInt(reader);
                    }
                    else if (type == typeof(float))
                    {
                        settingsMap[entry.Key] = parseFloat(reader);
                    }
                    else
                    {
                        settingsMap[entry.Key] = reader.ReadLine();
                    }
                }

                reader.Close();

            }
            catch (Exception e)
            {
                Debug.Log("Hubo un error inesperado");
                Debug.Log(e);

                reader.Close();

                setDefaults();
                saveFile();
            }
        }
    }

    private void setDefaults()
    {
        foreach(var entry in defaultSettings)
        {
            settingsMap[entry.Key] = defaultSettings.GetValueOrDefault(entry.Key);
        }
    }

    private void saveFile()
    {
        StreamWriter writer = new StreamWriter(this.file.FullName, false);

        foreach(object value in settingsMap.Values)
        {
            writer.WriteLine(value);
        }

        writer.Close();
    }

    private KeyCode parseKeyCode(StreamReader r)
    {
        return (KeyCode) Enum.Parse(typeof(KeyCode), r.ReadLine());
    }

    private int parseInt(StreamReader r)
    {
        return int.Parse(r.ReadLine());
    }

    private float parseFloat(StreamReader r)
    {
        return float.Parse(r.ReadLine());
    }

    public object getSetting(SettingName name)
    {
        return settingsMap[name];
    }

    public void setSetting(SettingName name, KeyCode code)
    {
        settingsMap[name] = code;
        this.saveFile();
    }

    public void setSetting(SettingName name, float value)
    {
        settingsMap[name] = value;
        this.saveFile();
    }

    public void setSetting(SettingName name, int value)
    {
        settingsMap[name] = value;
        this.saveFile();
    }

    public bool Holding(Settings.SettingName name)
    {
        return Input.GetKey((KeyCode) getSetting(name));
    }
    public bool Down(Settings.SettingName name)
    {
        return Input.GetKeyDown((KeyCode) getSetting(name));
    }

    public bool Up(Settings.SettingName name)
    {
        return Input.GetKeyUp((KeyCode) getSetting(name));
    }
}
