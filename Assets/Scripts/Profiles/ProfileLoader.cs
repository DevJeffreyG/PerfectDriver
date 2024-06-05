using TMPro;
using UnityEngine;

public class ProfileLoader : MonoBehaviour
{
    [SerializeField] private GameObject selectedProfile;
    private ProfileController profileController;
    private void Start()
    {
        profileController = ProfileController.getInstance();
    }

    private void Update()
    {
        selectedProfile.GetComponent<TMP_Text>().text = ProfileController.getProfile().getName();
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
