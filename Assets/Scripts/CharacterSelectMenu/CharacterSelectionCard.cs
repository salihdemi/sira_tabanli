using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionCard : MonoBehaviour
{
    public PersistanceStats stats;

    public Image portraitImage;
    public GameObject selectionFrame; // Seçildiðinde yanan görsel
    //public TextMeshProUGUI healthText;

    public Button addPartyButton;
    public Button openCharacterMenuButton;


    private CharacterSelectMenu characterSelectMenu;




    public void Setup(PersistanceStats stats, CharacterSelectMenu characterSelectMenu)
    {
        this.stats = stats;
        this.characterSelectMenu = characterSelectMenu;

        portraitImage.sprite = stats.sprite;
        //healthText.text = stats.currentHealth + " " + stats.maxHealth;
        selectionFrame.SetActive(stats.isInParty);
    }

    public void OnClickAddPartyButton()
    {
        if (stats.isInParty && PartyManager.partyStats.Count > 1)
        {
            PartyManager.TryToRemoveFromParty(stats);
            selectionFrame.SetActive(false);
        }
        else if (!stats.isInParty && PartyManager.partyStats.Count < 4)
        {
            PartyManager.TryAddToParty(stats);
            selectionFrame.SetActive(true);
        }
    }
    public void OnClickOpenCharacterMenuButton()
    {
        characterSelectMenu.OpenCharacterMenu(stats);
    }


}