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

    public List<CharacterSelectionCard> _cardPool = new List<CharacterSelectionCard>();
    public void OnEnable()
    {
        RefreshMenu();
    }

    private void RefreshMenu()
    {
        // 1. Önce havuza daha önce girmiþ tüm kartlarý pasife çek
        foreach (var card in _cardPool) card.gameObject.SetActive(false);
        

        var allies = PartyManager.allUnlockedAllies;

        // 2. Gereken kart sayýsý kadar döngü kur
        for (int i = 0; i < allies.Count; i++)
        {
            CharacterSelectionCard card;

            // Havuzda hali hazýrda bir kart varsa onu kullan
            if (i < _cardPool.Count) card = _cardPool[i];

            // Havuz yetersizse yeni bir tane oluþtur ve havuza ekle
            else
            {
                GameObject go = Instantiate(characterCardPrefab, contentArea);
                card = go.GetComponent<CharacterSelectionCard>();
                _cardPool.Add(card);
            }

            // Kartý görünür yap ve bilgilerini tazele
            card.gameObject.SetActive(true);
            card.Setup(allies[i], this);

            // Bonus: Kartlarýn sýrasýný korumak için (Layout Group kullanýyorsan)
            card.transform.SetAsLastSibling();
        }
    }


    public void OpenCharacterMenu(PersistanceStats stats)
    {
        characterMenu.gameObject.SetActive(true);
        characterMenu.OpenCharacterMenu(stats);
    }
}