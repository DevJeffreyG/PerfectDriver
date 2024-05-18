using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ProfileController : MonoBehaviour
{
    public static Profile profile;
    private DoubleCircularList profiles = new DoubleCircularList();

    void Start()
    {
        Directory.CreateDirectory(Paths.SETTINGS_PATH);
        DirectoryInfo dir = Directory.CreateDirectory(Paths.PROFILE_PATH);

        // Buscar todos los perfiles existentes en el path
        FileInfo[] profileFiles = dir.GetFiles();

        Debug.Log("Hay "+ profileFiles.Length + " perfiles guardados");

        if(profileFiles.Length > 0)
        {
            foreach(FileInfo profile in profileFiles)
            {
                this.profiles.Append(new Profile(profile));
            }
        } else
        {
            this.profiles.Append(new Profile("default"));
        }

    }

    void Update()
    {
        if(profile == null)
        {
            profile = (Profile) this.profiles.getHead().getData();
            Debug.Log("El perfil en uso ya no es nulo");
        }
    }
}
