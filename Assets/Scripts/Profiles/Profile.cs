using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Profile
{
    private FileInfo profileFile;
    private FileInfo settingsFile;
    private Settings settings;
   
    private String profilePath, settingsPath;
    public enum ProfileData
    {
        Name, SettingsPath, MaxPoints, TimesPlayed, CompletedTutorial
    }

    private Dictionary<ProfileData, object> defaultData, data;

    private void createMap(ref Dictionary<ProfileData, object> map)
    {
        if(map == null)
        {
            map = new Dictionary<ProfileData, object>();
        }

        map.Add(ProfileData.Name, "Perfil predeterminado");
        map.Add(ProfileData.SettingsPath, "");
        map.Add(ProfileData.MaxPoints, 0);
        map.Add(ProfileData.TimesPlayed, 0);
        map.Add(ProfileData.CompletedTutorial, false);
    }

    private void setDefaults()
    {
        foreach (var entry in defaultData)
        {
            data[entry.Key] = defaultData.GetValueOrDefault(entry.Key);
        }
    }

    public object getData(ProfileData name)
    {
        return data[name];
    }

    public Profile(String name, String fileName)
    {
        this.createMap(ref this.data);
        this.createMap(ref this.defaultData); 
        
        Directory.CreateDirectory(Paths.SETTINGS_PATH);
        DirectoryInfo dir = Directory.CreateDirectory(Paths.PROFILE_PATH);

        this.profilePath = Path.Combine(Paths.PROFILE_PATH, fileName + ".txt");
        this.settingsPath = Path.Combine(Paths.SETTINGS_PATH, fileName + ".txt");
        
        File.Create(profilePath).Close();
        File.Create(settingsPath).Close();

        this.profileFile = new FileInfo(profilePath);

        this.settingsFile = new FileInfo(settingsPath);
        this.settings = new Settings(this.settingsFile);

        this.initData(ProfileData.Name, name);
        this.initData(ProfileData.SettingsPath, settingsPath);
        this.saveFile();
    }

    public Profile(FileInfo file)
    {
        this.createMap(ref this.data);
        this.createMap(ref this.defaultData); 
        
        this.profileFile = file;
        this.readFile();
    }

    private Profile(String name, String profilePath, String settingsPath)
    {
        this.createMap(ref this.data);
        this.createMap(ref this.defaultData); 
        
        this.profileFile = new FileInfo(profilePath);
        this.readFile();

        this.initData(ProfileData.Name, name);
        this.initData(ProfileData.SettingsPath, settingsPath);

        this.settingsFile = new FileInfo((string) this.getData(ProfileData.SettingsPath));
        this.settings = new Settings(this.settingsFile);

        this.saveFile();
    }

    public FileInfo getFileInfo ()
    {
        return profileFile;
    }

    public String getName()
    {
        return (string) this.getData(ProfileData.Name);
    }

    public Settings getSettings()
    {
        return this.settings;
    }

    private void initData(ProfileData name, string value)
    {
        data[name] = value;
    }

    public void setData(ProfileData name, string value)
    {
        data[name] = value;
        this.saveFile();
    }

    public void setData(ProfileData name, int value)
    {
        data[name] = value;
        this.saveFile();
    }

    public void setData(ProfileData name, bool value)
    {
        data[name] = value;
        this.saveFile();
    }

    public void changeName(String newName)
    {
        this.setData(ProfileData.Name, newName);
    }

    public Profile duplicate(String fileName)
    {
        File.Copy(this.profileFile.FullName, Path.Combine(Paths.PROFILE_PATH, fileName + ".txt"));
        File.Copy(this.settingsFile.FullName, Path.Combine(Paths.SETTINGS_PATH, fileName + ".txt"));

        Profile newProfile = new Profile(getName() + " copy", Path.Combine(Paths.PROFILE_PATH, fileName + ".txt"), Path.Combine(Paths.SETTINGS_PATH, fileName + ".txt"));
        return newProfile;
    }

    public void delete()
    {
        File.Delete(this.profileFile.FullName);
        File.Delete(this.settingsFile.FullName);
    }

    public void saveFile()
    {
        StreamWriter writer = new StreamWriter(this.profileFile.FullName, false);

        foreach (object value in data.Values)
        {
            writer.WriteLine(value);
        }

        writer.Close();
    }

    private void readFile()
    {
        StreamReader reader = new StreamReader(this.profileFile.FullName);

        if (File.ReadLines(this.profileFile.FullName).Count() != defaultData.Count)
        {
            reader.Close();

            this.setDefaults();
            this.saveFile();
        }
        else
        {
            try
            {
                foreach (var entry in data.ToList())
                {
                    Type type = entry.Value.GetType();

                    if (type == typeof(bool))
                    {
                        data[entry.Key] = Parsers.parseBool(reader);
                    }else
                    if (type == typeof(int))
                    {
                        data[entry.Key] = Parsers.parseInt(reader);
                    }
                    else
                    {
                        data[entry.Key] = reader.ReadLine();
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

        this.settingsPath = (string) this.getData(ProfileData.SettingsPath);
        if (!File.Exists(this.settingsPath)) this.settingsPath = Path.Combine(Paths.SETTINGS_PATH, this.profileFile.Name);

        this.settingsFile = new FileInfo(this.settingsPath);
        this.settings = new Settings(this.settingsFile);

        reader.Close();
    }
}
