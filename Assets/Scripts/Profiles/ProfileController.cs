using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProfileController : MonoBehaviour
{
    private FileInfo globalFile;
    public static Profile profile;
    private DoubleCircularList profiles = new DoubleCircularList();
    private int LastId = 0;

    void Start()
    {
        Directory.CreateDirectory(Paths.SETTINGS_PATH);
        DirectoryInfo dir = Directory.CreateDirectory(Paths.PROFILE_PATH);
        globalFileWorker();

        // Buscar todos los perfiles existentes en el path
        FileInfo[] profileFiles = dir.GetFiles().OrderByDescending(x => x.Name).Reverse().ToArray();

        Debug.Log("Hay "+ profileFiles.Length + " perfiles guardados");

        if(profileFiles.Length > 0)
        {
            foreach(FileInfo profile in profileFiles)
            {
                this.profiles.Append(new Profile(profile));
            }
        } else
        {
            this.profiles.Append(new Profile("Perfil predeterminado", this.LastId.ToString()));
            this.LastId++;
        }
    }

    void Update()
    {
        if(profile == null)
        {
            
            int pos = 0;
            StreamReader reader = new StreamReader(globalFile.FullName);
            try
            {
                pos = Int32.Parse(reader.ReadLine());
                this.LastId = Int32.Parse(reader.ReadLine());
            }
            catch (Exception e)
            {
                reader.Close();
                Debug.Log(e);
                globalFile.Delete();
                this.globalFileWorker();
            }

            this.profiles.GoTo(pos);
            reader.Close();
            this.updateGlobalFile(); // Cambiar la informacion dentro del archivo en caso de ser necesario

            profile = (Profile) this.profiles.getPointer().getData();
            Debug.Log("El perfil en uso ya no es nulo");
        } else
        {
            GameObject.Find("ProfileSelected").GetComponent<TMP_Text>().text = profile.getName();
        }
    }

    public void createProfile()
    {
        this.LastId++;
        this.profiles.Append(new Profile("Perfil " + this.profiles.Length().ToString(), this.LastId.ToString()));
        this.profiles.PointTail();

        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    public void duplicateProfile()
    {
        this.LastId++;
        Profile profileToDup = (Profile) this.profiles.getPointer().getData();
        this.profiles.Append(profileToDup.duplicate(this.LastId.ToString()));
        this.profiles.PointTail();

        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    public void deleteProfile()
    {
        Debug.Log(this.profiles.Length());
        if(this.profiles.Length() != 1)
        {
            Profile profileToDel = (Profile)this.profiles.getPointer().getData();
            this.profiles.DeletePointer();
            profileToDel.delete();

            this.updateGlobalFile();
            profile = (Profile)this.profiles.getPointer().getData();
        } else
        {
            Debug.Log("No se eliminó el perfil, sólo queda uno");
        }
        
    }

    public void nextProfile()
    {
        this.profiles.Next();

        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    public void prevProfile()
    {
        this.profiles.Prev();

        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    private void globalFileWorker()
    {
        String path = Path.Combine(Paths.GLOBALFILE_PATH);
        if(!File.Exists(path))
        {
            File.Create(path).Close();
            this.updateGlobalFile();
        }

        globalFile = new FileInfo(path);
    }

    private void updateGlobalFile()
    {
        StreamWriter writer = new StreamWriter(Paths.GLOBALFILE_PATH, false);
        writer.WriteLine(this.profiles.Pos().ToString());
        writer.WriteLine(this.LastId.ToString());
        writer.Close();
    }
}
