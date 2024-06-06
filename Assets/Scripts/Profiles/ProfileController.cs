using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProfileController : MonoBehaviour
{
    private static ProfileController instance;
    private static Profile profile;
    private static int LastId = 0;
    private static bool loadedLastProfile = false;
    
    private FileInfo globalFile;
    private DoubleCircularList profiles = new DoubleCircularList();

    public static ProfileController getInstance()
    {
        return instance;
    }

    public static Profile getProfile()
    {
        if (profile == null && !loadedLastProfile)
        {
            Debug.Log("Debugging");
            // Esto sólo debería pasar cuando se carga una escena de primero que NO es el MainMenu (donde está el iniciador de ProfileController)
            profile = new Profile("Perfil predeterminado", LastId.ToString());
        }        
        
        return profile;
    }

    void Awake()
    {
        instance = this;
        Directory.CreateDirectory(Paths.SETTINGS_PATH);
        DirectoryInfo dir = Directory.CreateDirectory(Paths.PROFILE_PATH);
        globalFileWorker();

        // Buscar todos los perfiles existentes en el path
        FileInfo[] profileFiles;
        try
        {
            profileFiles = dir.GetFiles().OrderByDescending(x => Int32.Parse(x.Name.Replace(".txt", ""))).Reverse().ToArray();
        }
        catch (Exception)
        {
            profileFiles = dir.GetFiles().OrderByDescending(x => x.Name.Replace(".txt", "")).Reverse().ToArray(); 
            Debug.Log("Hubo un error ordenando los perfiles, probablemente estarán desordenados");
        }


        Debug.Log("Hay "+ profileFiles.Length + " perfiles guardados");

        if(profileFiles.Length > 0)
        {
            foreach(FileInfo profile in profileFiles)
            {
                this.profiles.Append(new Profile(profile));
            }
        } else
        {
            this.profiles.Append(new Profile("Perfil predeterminado", LastId.ToString()));
            LastId++;
        }

        if(!loadedLastProfile) profile = (Profile) this.profiles.getPointer().getData();
    }

    void Update()
    {
        if(!loadedLastProfile)
        {
            int pos = 0;
            StreamReader reader = new StreamReader(globalFile.FullName);
            try
            {
                pos = Int32.Parse(reader.ReadLine());
                LastId = Int32.Parse(reader.ReadLine());
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
            loadedLastProfile = true;
        }               
    }

    public void createProfile()
    {
        LastId++;
        this.profiles.Append(new Profile("Perfil " + this.profiles.Length().ToString(), LastId.ToString()));
        this.profiles.PointTail();

        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    public void duplicateProfile()
    {
        LastId++;
        Profile profileToDup = (Profile) this.profiles.getPointer().getData();
        this.profiles.Append(profileToDup.duplicate(LastId.ToString()));
        this.profiles.PointTail();

        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    public void deleteProfile()
    {
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
        writer.WriteLine(LastId.ToString());
        writer.Close();
    }
}
