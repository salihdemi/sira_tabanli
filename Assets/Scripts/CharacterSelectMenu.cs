using UnityEngine;
using System.Collections.Generic;

public class CharacterSelectMenu : MonoBehaviour
{
    public PartyManager partyManager;
    public GameObject cardPrefab;
    public Transform contentArea; // ScrollView Content

    private List<CharacterSelectionCard> spawnedCards = new List<CharacterSelectionCard>();

    public void OnEnable()
    {
        //gameObject.SetActive(true);
        RefreshMenu();
    }

    private void RefreshMenu()
    {
        foreach (Transform child in contentArea) Destroy(child.gameObject);
        spawnedCards.Clear();

        foreach (var ally in partyManager.allUnlockedAllies)
        {
            GameObject go = Instantiate(cardPrefab, contentArea);
            var card = go.GetComponent<CharacterSelectionCard>();
            card.Setup(ally, this);
            spawnedCards.Add(card);
        }

        foreach (var c in spawnedCards) c.UpdateUI();
    }

    public void HandleSelection(CharacterSelectionCard card)
    {
        if (card.myStats.isInParty && partyManager.partyStats.Count > 1)
        {
            partyManager.TryToRemoveFromParty(card.myStats);
        }
        else if(!card.myStats.isInParty && partyManager.partyStats.Count < 4)
        {
            partyManager.TryAddToParty(card.myStats);
        }

        // Tüm kartlarý güncelle (Bir karakter eklenince hepsi kontrol etsin)
        foreach (var c in spawnedCards) c.UpdateUI();
    }
}