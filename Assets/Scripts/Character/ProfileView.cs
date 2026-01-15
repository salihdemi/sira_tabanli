using UnityEngine;
using UnityEngine.UI;

public class ProfileView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Profile boundProfile;
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