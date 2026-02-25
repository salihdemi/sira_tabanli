using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileButtonHandler : MonoBehaviour
{
    // Statik Event: Herhangi bir profile týklandýðýnda bu profili yayýnlar.
    // TargetingSystem bu sesi dinleyecek.

    [SerializeField] private Button button;
    [SerializeField] private Profile boundProfile;

    public void SetSelectable(bool state)
    {
        button.interactable = state;
    }

    public void OnProfileButtonPressed()
    {
        if (boundProfile == null) return;


        TargetingSystem.OnProfileClicked(boundProfile);
    }
}