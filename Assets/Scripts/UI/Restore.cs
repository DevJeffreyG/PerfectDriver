using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restore : MonoBehaviour
{
    // Start is called before the first frame update
    public void restoreF(){
        ProfileController.getProfile().getSettings().resetSettings();
    }
}
