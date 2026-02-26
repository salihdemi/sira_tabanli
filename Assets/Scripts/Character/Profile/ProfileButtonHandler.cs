using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileButtonHandler : MonoBehaviour
{
    // Statik Event: Herhangi bir profile týklandýðýnda bu profili yayýnlar.
    // TargetingSystem bu sesi dinleyecek.

    [SerializeField] private Button button;
    [SerializeField] public Profile profile;

    public void SetSelectable(bool state)
    {
        button.interactable = state;
    }

    public void OnProfileButtonPressed()
    {
        if (profile == null) return;


        TargetingSystem.OnProfileClicked(profile);
    }
}