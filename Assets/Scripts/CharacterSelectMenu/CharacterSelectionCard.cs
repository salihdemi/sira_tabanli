using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionCard : MonoBehaviour
{
    public PersistanceStats stats;

    public Image portraitImage;
    public GameObject selectionFrame; // Seçildiðinde yanan görsel
    public TextMeshProUGUI healthText;
    public Button weaponSocket;
    public Button itemSocket;
    public Button charmSocket;


    private CharacterSelectMenu characterSelectMenu;

    public void Setup(PersistanceStats stats, CharacterSelectMenu manager)
    {
        this.stats = stats;
        characterSelectMenu = manager;

        portraitImage.sprite = stats.sprite;
        healthText.text = stats.currentHealth + " " + stats.maxHealth;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Eðer bu karakter partyStats listesinde varsa çerçeveyi aç
        selectionFrame.SetActive(stats.isInParty);
        //Debug.Log(myStats._name +" "+myStats.isInParty);
    }

    public void OnClick() // Butona baðla
    {
        characterSelectMenu.HandleSelection(this);
    }

    public void OnClickWeaponSocket()
    {
        characterSelectMenu.OpenOrCloseWeaponsTab(stats, stats.weaponType);
    }
    public void OnClickItemSocket()
    {
        characterSelectMenu.OpenOrCloseItemsTab(stats);
    }
    public void OnClickCharmSocket()
    {
        characterSelectMenu.OpenOrCloseCharmsTab(stats);
    }
}