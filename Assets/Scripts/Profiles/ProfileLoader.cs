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
        selectedProfile.GetComponent<TMP_Text>().text = ProfileController.getProfile().getName();
        points.text = ""+ProfileController.getProfile().getData(Profile.ProfileData.MaxPoints);
        times.text = ""+ProfileController.getProfile().getData(Profile.ProfileData.TimesPlayed);
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
