
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
    public static readonly KeyCode D_INTERACT = KeyCode.F;
    public static readonly KeyCode D_ZOOM = KeyCode.Mouse1;
    public static readonly KeyCode D_BACK = KeyCode.Escape;

    public static readonly KeyCode D_DIRECTIONAL_RIGHT = KeyCode.E;
    public static readonly KeyCode D_DIRECTIONAL_LEFT = KeyCode.Q;
    public static readonly KeyCode D_TOGGLE_ENGINE = KeyCode.R;
    public static readonly KeyCode D_TOGGLE_HANDBRAKE = KeyCode.Space;
    public static readonly KeyCode D_TOGGLE_LIGHTS = KeyCode.C;
    public static readonly KeyCode D_GEAR_DOWN = KeyCode.Alpha1;
    public static readonly KeyCode D_GEAR_UP = KeyCode.Alpha2;
    public static readonly KeyCode D_STABILIZE_STEER_WHEEL = KeyCode.CapsLock;
    public static readonly KeyCode D_FASTER_STEER = KeyCode.LeftShift;
    public static readonly KeyCode D_HORN = KeyCode.Mouse0;
    public static readonly KeyCode D_EMERGENCY_LIGHTS = KeyCode.Backspace;

    public static readonly float D_CAMERA_SENS = 5f;
    public static readonly int D_MAX_FPS = 120;
    public static readonly float D_CAMERA_DISTANCE = 1000f;
    public static readonly float D_SFX_VOLUME = 1f;
    public static readonly float D_BG_VOLUME = 1f;

    public static readonly bool D_ONLY_KEYBOARD = false;
    public static readonly KeyCode D_UP_CAM = KeyCode.UpArrow;
    public static readonly KeyCode D_LEFT_CAM = KeyCode.LeftArrow;
    public static readonly KeyCode D_DOWN_CAM = KeyCode.DownArrow;
    public static readonly KeyCode D_RIGHT_CAM = KeyCode.RightArrow;

    public static readonly bool D_SHOW_HOUSES = true;
    public static readonly bool D_SHOW_TREES = true;
    public static readonly float D_TREES_RENDER_DISTANCE = 1f;

    private FileInfo file;

    public enum SettingName
    {
        Accelerate, Left, Brake, Right, Jump, Interact, DirectionalRight, DirectionalLeft, ToggleEngine, ToggleHandbrake, ToggleLights, CameraSens, MaxFPS,
        GearUp, GearDown, StabilizeSteerWheel, FasterSteering, Zoom, Horn, EmergencyLights, CameraDistance, SFX_Volume, BG_Volume, OnlyKeyboard, UpCam,
        LeftCam, DownCam, RightCam, ShowHouses, ShowTrees, TreesRenderDistance, Back
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
        map.Add(SettingName.Interact, D_INTERACT);
        map.Add(SettingName.Zoom, D_ZOOM);
        map.Add(SettingName.Back, D_BACK);

        map.Add(SettingName.DirectionalRight, D_DIRECTIONAL_RIGHT);
        map.Add(SettingName.DirectionalLeft, D_DIRECTIONAL_LEFT);
        map.Add(SettingName.EmergencyLights, D_EMERGENCY_LIGHTS);
        map.Add(SettingName.ToggleEngine, D_TOGGLE_ENGINE);
        map.Add(SettingName.ToggleHandbrake, D_TOGGLE_HANDBRAKE);
        map.Add(SettingName.ToggleLights, D_TOGGLE_LIGHTS);
        map.Add(SettingName.GearDown, D_GEAR_DOWN);
        map.Add(SettingName.GearUp, D_GEAR_UP);
        map.Add(SettingName.StabilizeSteerWheel, D_STABILIZE_STEER_WHEEL);
        map.Add(SettingName.FasterSteering, D_FASTER_STEER);
        map.Add(SettingName.Horn, D_HORN);

        map.Add(SettingName.CameraSens, D_CAMERA_SENS);
        map.Add(SettingName.MaxFPS, D_MAX_FPS);

        map.Add(SettingName.OnlyKeyboard, D_ONLY_KEYBOARD);
        map.Add(SettingName.UpCam, D_UP_CAM);
        map.Add(SettingName.LeftCam, D_LEFT_CAM);
        map.Add(SettingName.DownCam, D_DOWN_CAM);
        map.Add(SettingName.RightCam, D_RIGHT_CAM);

        map.Add(SettingName.CameraDistance, D_CAMERA_DISTANCE);
        map.Add(SettingName.SFX_Volume, D_SFX_VOLUME);
        map.Add(SettingName.BG_Volume, D_BG_VOLUME);

        map.Add(SettingName.ShowHouses, D_SHOW_HOUSES);
        map.Add(SettingName.ShowTrees, D_SHOW_TREES);
        map.Add(SettingName.TreesRenderDistance, D_TREES_RENDER_DISTANCE);
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
                        settingsMap[entry.Key] = Parsers.parseKeyCode(reader);
                    }
                    else if (type == typeof(int))
                    {
                        settingsMap[entry.Key] = Parsers.parseInt(reader);
                    }
                    else if (type == typeof(float))
                    {
                        settingsMap[entry.Key] = Parsers.parseFloat(reader);
                    }
                    else if(type == typeof(bool))
                    {
                        settingsMap[entry.Key] = Parsers.parseBool(reader);
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

    public void resetSettings(){
        setDefaults();
        saveFile();
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

    public object getSetting(SettingName name)
    {
        return settingsMap[name];
    }

    public void setSetting(SettingName name, KeyCode code, out bool problem)
    {
        settingsMap[name] = code;
        problem = false;

        List<KeyCode> keys = new List<KeyCode>();
        foreach (var key in settingsMap.Values)
        {
            if(key.GetType() == typeof(KeyCode))
            {
                if(!keys.Contains((KeyCode)key))
                {
                    keys.Add((KeyCode)key);
                } else
                {
                    problem = true;
                    break;
                }
                
            }
        }

        if(!problem) this.saveFile();
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
    public void setSetting(SettingName name, bool value)
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
