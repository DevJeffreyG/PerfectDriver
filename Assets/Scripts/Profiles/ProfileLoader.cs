using TMPro;
using UnityEngine;

public class ProfileLoader : MonoBehaviour
{
    [SerializeField] private GameObject selectedProfile;
    [SerializeField] private TMPro.TMP_Text points;
    [SerializeField] private TMPro.TMP_Text times;
    private ProfileController profileController;
    private Profile selected;
    private void Start()
    {
        profileController = ProfileController.getInstance();
    }

    private void Update()
    {
        selected = ProfileController.getProfile();
        
        selectedProfile.GetComponent<TMP_Text>().text = selected.getName();
        points.text = selected.getData(Profile.ProfileData.MaxPoints).ToString();
        times.text = selected.getData(Profile.ProfileData.TimesPlayed).ToString();
    }

    public void nextProfile()
    {        
        profileController.nextProfile();
    }

    public void prevProfile()
    {
        profileController.prevProfile();
    }

    public void createProfile()
    {
        profileController.createProfile();
    }
    
    public void deleteProfile()
    {
        profileController.deleteProfile();
    }

    public void duplicateProfile()
    {
        profileController.duplicateProfile();
    }
}
