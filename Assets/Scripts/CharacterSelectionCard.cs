using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionCard : MonoBehaviour
{
    public PersistanceStats myStats;
    public Image portrait;
    public GameObject selectionFrame; // Seçildiðinde yanan görsel
    public TextMeshProUGUI healthText;

    private CharacterSelectMenu menuManager;

    public void Setup(PersistanceStats stats, CharacterSelectMenu manager)
    {
        myStats = stats;
        menuManager = manager;

        portrait.sprite = stats.sprite;
        healthText.text = stats.currentHealth + " " + stats.maxHealth;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Eðer bu karakter partyStats listesinde varsa çerçeveyi aç
        selectionFrame.SetActive(myStats.isInParty);Debug.Log(myStats._name +" "+myStats.isInParty);
    }

    public void OnClick() // Butona baðla
    {
        menuManager.HandleSelection(this);
    }
}