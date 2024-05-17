using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Profile
{
    private FileInfo profileFile;
    private FileInfo keybindsFile;
    private Keybinds keybinds;
   
    private String profileName;

    public Profile(String name)
    {
        String profilePath = Path.Combine(Paths.PROFILE_PATH, name + ".txt");
        String keybindPath = Path.Combine(Paths.KEYBINDS_PATH, name + ".txt");

        File.Create(profilePath).Close();
        File.Create(keybindPath).Close();

        this.profileFile = new FileInfo(profilePath);

        this.profileName = name;
        this.keybindsFile = new FileInfo(keybindPath);
        this.keybinds = new Keybinds(this.keybindsFile);

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

    public Keybinds getKeybinds()
    {
        return this.keybinds;
    }

    public void saveFile()
    {
        StreamWriter writer = new StreamWriter(this.profileFile.FullName, false);
        writer.WriteLine(profileName);
        writer.WriteLine(this.keybindsFile.FullName);

        writer.Close();
    }

    private void readFile()
    {
        StreamReader reader = new StreamReader(this.profileFile.FullName);

        this.profileName = reader.ReadLine();
        this.keybindsFile = new FileInfo(reader.ReadLine());
        this.keybinds = new Keybinds(this.keybindsFile);

        reader.Close();
    }
}
