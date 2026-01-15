using UnityEngine;
using UnityEngine.UI;

public class ProfileView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Profile boundProfile;
    private void Awake()
    {
        Setup(boundProfile);
    }
    public void Setup(Profile profile)
    {
        boundProfile = profile;
        profile.view = this;
        // Eski dinleyicileri temizleyip yenisini ekliyoruz
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnProfileButtonPressed());
    }

    public void SetSelectable(bool state)
    {
        button.interactable = state;
        // Burada görsel efektler eklenebilir (Örn: Parlama efekti)
    }
    public void OnProfileButtonPressed()
    {
        Debug.Log(name);
        TargetingSystem.instance.OnProfileClicked(boundProfile);
    }
}