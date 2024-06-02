using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ProfileController : MonoBehaviour
{
    private FileInfo globalFile;
    public static Profile profile;
    private DoubleCircularList profiles = new DoubleCircularList();

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
            this.profiles.Append(new Profile("0"));
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
            //Debug.Log(profile.getName());
        }
    }

    public void createProfile()
    {
        this.profiles.Append(new Profile(this.profiles.Length().ToString()));
        this.profiles.PointTail();

        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    public void duplicateProfile()
    {
        Profile profileToDup = (Profile) this.profiles.getPointer().getData();
        this.profiles.Append(profileToDup.duplicate(this.profiles.Length().ToString()));
        this.profiles.PointTail();
        
        this.updateGlobalFile();
        profile = (Profile) this.profiles.getPointer().getData();
    }

    public void deleteProfile()
    {
        // TODO
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
        writer.Close();
    }
}
