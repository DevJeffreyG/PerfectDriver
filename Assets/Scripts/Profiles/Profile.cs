using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Profile
{
    private FileInfo profileFile;
    private FileInfo settingsFile;
    private Settings settings;
   
    private String profileName;

    public Profile(String name)
    {
        String profilePath = Path.Combine(Paths.PROFILE_PATH, name + ".txt");
        String keybindPath = Path.Combine(Paths.SETTINGS_PATH, name + ".txt");

        File.Create(profilePath).Close();
        File.Create(keybindPath).Close();

        this.profileFile = new FileInfo(profilePath);

        this.profileName = name;
        this.settingsFile = new FileInfo(keybindPath);
        this.settings = new Settings(this.settingsFile);

        this.saveFile();
    }

    public Profile(FileInfo file)
    {
        this.profileFile = file;
        this.readFile();
    }

    public FileInfo getFileInfo ()
    {
        return profileFile;
    }

    public Settings getSettings()
    {
        return this.settings;
    }

    public void saveFile()
    {
        StreamWriter writer = new StreamWriter(this.profileFile.FullName, false);
        writer.WriteLine(profileName);
        writer.WriteLine(this.settingsFile.FullName);

        writer.Close();
    }

    private void readFile()
    {
        StreamReader reader = new StreamReader(this.profileFile.FullName);

        this.profileName = reader.ReadLine();
        this.settingsFile = new FileInfo(reader.ReadLine());
        this.settings = new Settings(this.settingsFile);

        reader.Close();
    }
}
