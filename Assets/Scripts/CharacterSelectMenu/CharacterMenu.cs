using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterMenu : MonoBehaviour
{
    private PersistanceStats currentCharacter;

    [Header("UI References")]
    public Image characterPortrait;
    public TextMeshProUGUI healthText, manaText, staminaText, strengthText, technicalText, focusText, speedText;
    public GameObject skillMenu, weaponMenu, itemMenu, talismanMenu;

    [Header("Prefabs")]
    public EquipableSelectionCard equipableCardPrefab;

    // --- POOLING SİSTEMİ ---
    private List<EquipableSelectionCard> _cardPool = new List<EquipableSelectionCard>();

    public void OpenCharacterMenu(PersistanceStats stats)
    {
        currentCharacter = stats;
        gameObject.SetActive(true);
        characterPortrait.sprite = currentCharacter.sprite;
        WriteStats();
    }

    private void WriteStats()
    {
        healthText.text = currentCharacter.currentHealth + "/" + currentCharacter.maxHealth;
        manaText.text = currentCharacter.currentMana + "/" + currentCharacter.maxMana;
        staminaText.text = currentCharacter.currentStamina + "/" + currentCharacter.maxStamina;

        strengthText.text = currentCharacter.strength.ToString();
        technicalText.text = currentCharacter.technical.ToString();
        focusText.text = currentCharacter.focus.ToString();
        speedText.text = currentCharacter.speed.ToString();
    }

    // --- TAB YÖNETİMİ (DRY PRENSİBİ) ---
    public void OnSkillMenuButtonPressed() => ToggleTab(skillMenu, () => OpenSkillsTab(currentCharacter));
    public void OnWeaponMenuButtonPressed() => ToggleTab(weaponMenu, () => OpenWeaponsTab(currentCharacter));
    public void OnItemMenuButtonPressed() => ToggleTab(itemMenu, () => OpenItemsTab(currentCharacter));
    public void OnTalismanMenuButtonPressed() => ToggleTab(talismanMenu, () => OpenTalismansTab(currentCharacter));

    private void ToggleTab(GameObject targetMenu, UnityAction openAction)
    {
        bool isActive = targetMenu.activeSelf;

        // Tüm menüleri kapat
        skillMenu.SetActive(false);
        weaponMenu.SetActive(false);
        itemMenu.SetActive(false);
        talismanMenu.SetActive(false);

        // Eğer kapalıysa aç ve içeriği doldur
        if (!isActive)
        {
            targetMenu.SetActive(true);
            DeactivateAllCards(); // Havuzu temizle
            openAction.Invoke();
        }
    }

    // --- İÇERİK DOLDURMA (POOLING KULLANIMI) ---

    public void OpenSkillsTab(PersistanceStats stats)
    {
        Transform container = skillMenu.transform.GetChild(0);
        foreach (Skill skill in stats.unlockedSkills)
        {
            EquipableSelectionCard card = GetCardFromPool(container);
            card.image.sprite = skill.sprite; // Skill'de sprite olduğunu varsayıyorum
            card.text.text = skill._name;
            SetupSkillButton(card, skill, stats);
        }
    }

    private void SetupSkillButton(EquipableSelectionCard card, Skill skill, PersistanceStats stats)
    {
        bool isEquipped = stats.currentSkills.Contains(skill);
        card.button.onClick.RemoveAllListeners();

        if (isEquipped)
        {
            card.button.onClick.AddListener(() => {
                stats.TryUnequipSkill(skill);
                SetupSkillButton(card, skill, stats); // Durumu güncelle
            });
        }
        else
        {
            card.button.onClick.AddListener(() => {
                stats.TryEquipSkill(skill);
                SetupSkillButton(card, skill, stats); // Durumu güncelle
            });
        }
    }

    public void OpenWeaponsTab(PersistanceStats stats)
    {
        Transform container = weaponMenu.transform.GetChild(0);
        foreach (Weapon weapon in InventoryManager.ownedWeapons)
        {
            if (weapon.type == currentCharacter.weaponType)
            {
                EquipableSelectionCard card = GetCardFromPool(container);
                card.image.sprite = weapon.sprite;
                card.text.text = weapon.name;

                bool isEquipped = InventoryManager.IsEquipped(weapon);
                card.button.interactable = !isEquipped;

                if (!isEquipped)
                {
                    card.button.onClick.RemoveAllListeners();
                    card.button.onClick.AddListener(() => {
                        InventoryManager.Equip(stats, weapon);
                        weaponMenu.SetActive(false);
                    });
                }
            }
        }
    }

    public void OpenItemsTab(PersistanceStats stats)
    {
        Transform container = itemMenu.transform.GetChild(0);
        foreach (Item item in InventoryManager.ownedItems)
        {
            EquipableSelectionCard card = GetCardFromPool(container);
            card.image.sprite = item.sprite;
            card.text.text = item.name;

            bool isEquipped = InventoryManager.IsEquipped(item);
            card.button.interactable = !isEquipped;

            if (!isEquipped)
            {
                card.button.onClick.RemoveAllListeners();
                card.button.onClick.AddListener(() => {
                    InventoryManager.Equip(stats, item);
                    itemMenu.SetActive(false);
                });
            }
        }
    }

    public void OpenTalismansTab(PersistanceStats stats)
    {
        Transform container = talismanMenu.transform.GetChild(0);
        foreach (Talisman talisman in InventoryManager.ownedTalismas)
        {
            EquipableSelectionCard card = GetCardFromPool(container);
            card.image.sprite = talisman.sprite;
            card.text.text = talisman.name;

            bool isEquipped = InventoryManager.IsEquipped(talisman);
            card.button.interactable = !isEquipped;

            if (!isEquipped)
            {
                card.button.onClick.RemoveAllListeners();
                card.button.onClick.AddListener(() => {
                    InventoryManager.Equip(stats, talisman);
                    WriteStats();
                    talismanMenu.SetActive(false);
                });
            }
        }
    }

    // --- YARDIMCI POOL METODLARI ---

    private EquipableSelectionCard GetCardFromPool(Transform parent)
    {
        // Havuzda boşta kart var mı bak
        foreach (EquipableSelectionCard card in _cardPool)
        {
            if (!card.gameObject.activeSelf)
            {
                card.transform.SetParent(parent);
                card.gameObject.SetActive(true);
                return card;
            }
        }

        // Yoksa yeni oluştur ve havuza ekle
        EquipableSelectionCard newCard = Instantiate(equipableCardPrefab, parent);
        _cardPool.Add(newCard);
        return newCard;
    }

    private void DeactivateAllCards()
    {
        foreach (EquipableSelectionCard card in _cardPool)
        {
            card.gameObject.SetActive(false);
        }
    }
}