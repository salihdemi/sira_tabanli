using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileView : MonoBehaviour
{
    // Statik Event: Herhangi bir profile týklandýðýnda bu profili yayýnlar.
    // TargetingSystem bu sesi dinleyecek.
    public static event Action<Profile> OnAnyProfileClicked;

    [SerializeField] private Button button;
    [SerializeField] private Profile boundProfile;

    public void SetSelectable(bool state)
    {
        button.interactable = state;
    }

    public void OnProfileButtonPressed()
    {
        if (boundProfile == null) return;

        Debug.Log(boundProfile.name + " týklandý, event fýrlatýlýyor.");

        OnAnyProfileClicked?.Invoke(boundProfile);
    }
}