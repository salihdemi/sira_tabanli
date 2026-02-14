using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterSelectMenu : MonoBehaviour
{
    public GameObject characterCardPrefab;

    public Transform contentArea;

    public CharacterMenu characterMenu;

    //private List<CharacterSelectionCard> spawnedCards = new List<CharacterSelectionCard>();

    public void OnEnable()
    {
        RefreshMenu();
    }

    private void RefreshMenu()
    {
        foreach (Transform child in contentArea) Destroy(child.gameObject);


        foreach (var ally in PartyManager.allUnlockedAllies)
        {
            GameObject go = Instantiate(characterCardPrefab, contentArea);
            CharacterSelectionCard card = go.GetComponent<CharacterSelectionCard>();
            card.Setup(ally, this);
            //spawnedCards.Add(card);
        }
    }
    
    
    public void OpenCharacterMenu(PersistanceStats stats)
    {
        characterMenu.gameObject.SetActive(true);
        characterMenu.OpenCharacterMenu(stats);
    }
}