using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CharacterSelectMenu : MonoBehaviour
{
    public GameObject characterCardPrefab;
    public WeaponSelectCard weaponCardPrefab;
    public ItemSelectCard ItemSelectCard;
    public CharmSelectCard charmSelectCard;
    public Transform contentArea; // ScrollView Content


    public GameObject weaponsTab;
    public GameObject itemsTab;
    public GameObject charmsTab;


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

        //foreach (Transform t in contentArea) t.GetComponent<CharacterSelectionCard>().UpdateUI();
    }

    public void HandleSelection(CharacterSelectionCard card)
    {
        Debug.Log("HandleSelection");
        if (card.stats.isInParty && PartyManager.partyStats.Count > 1)
        {
            PartyManager.TryToRemoveFromParty(card.stats);
        }
        else if(!card.stats.isInParty && PartyManager.partyStats.Count < 4)
        {
            PartyManager.TryAddToParty(card.stats);
        }

        // Tüm kartlarý güncelle (Bir karakter eklenince hepsi kontrol etsin)
        foreach (Transform t in contentArea) t.GetComponent<CharacterSelectionCard>().UpdateUI();//?
    }

    public void OpenOrCloseWeaponsTab(PersistanceStats stats, WeaponType weaponType)
    {
        if (weaponsTab.activeSelf)
        {
            weaponsTab.SetActive(false);
            return;
        }

        foreach (Transform child in weaponsTab.transform) Destroy(child.gameObject);

        weaponsTab.SetActive(true);
        //uygun olanlarý ac
        foreach (Weapon weapon in InventoryManager.weapons)
        {
            //tipi uygun mu, kullanýlýyor mu
            if(weapon.type == weaponType && !weapon.equipped)
            {
                WeaponSelectCard card = Instantiate(weaponCardPrefab, weaponsTab.transform);

                card.image.sprite = weapon.sprite;

                card.weapon = weapon;
                card.button.onClick.RemoveAllListeners();
                card.button.onClick.AddListener(() => card.OnCardClicked(stats));
                card.button.onClick.AddListener(() => weaponsTab.SetActive(false));
            }
        }
    }
    public void OpenOrCloseItemsTab(PersistanceStats stats)
    {
        if (itemsTab.activeSelf)
        {
            itemsTab.SetActive(false);
            return;
        }


        foreach (Transform child in itemsTab.transform) Destroy(child.gameObject);

        itemsTab.SetActive(true);


        foreach (Item item in InventoryManager.items)
        {
            //tipi uygun mu, kullanýlýyor mu
            if (!item.equipped)
            {
                ItemSelectCard card = Instantiate(ItemSelectCard, itemsTab.transform);

                card.image.sprite = item.sprite;

                card.item = item;
                card.button.onClick.RemoveAllListeners();
                card.button.onClick.AddListener(() => card.OnCardClicked(stats));
                card.button.onClick.AddListener(() => itemsTab.SetActive(false));
            }
        }
    }
    public void OpenOrCloseCharmsTab(PersistanceStats stats)
    {
        if (charmsTab.activeSelf)
        {
            charmsTab.SetActive(false);
            return;
        }


        foreach (Transform child in charmsTab.transform) Destroy(child.gameObject);

        charmsTab.SetActive(true);


        foreach (Charm charm in InventoryManager.charms)
        {
            //tipi uygun mu, kullanýlýyor mu
            if (!charm.equipped)
            {
                CharmSelectCard card = Instantiate(charmSelectCard, charmsTab.transform);

                card.image.sprite = charm.sprite;

                card.charm = charm;
                card.button.onClick.RemoveAllListeners();
                card.button.onClick.AddListener(() => card.OnCardClicked(stats));
                card.button.onClick.AddListener(() => charmsTab.SetActive(false));
            }
        }
    }
}