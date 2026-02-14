using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterMenu : MonoBehaviour
{
    private PersistanceStats currentCharacter;

    public Image characterPortrait;
    public TextMeshProUGUI healthText, manaText, staminaText, strengthText, technicalText, focusText, speedText;
    //public Toggle skillsToggle, weaponsToggle, itemsToggle, talismansToggle;
    public GameObject skillMenu, weaponMenu, itemMenu, talismanMenu;


    public EquipableSelectionCard equipableCardPrefab;

    public void OpenCharacterMenu(PersistanceStats stats)
    {
        currentCharacter = stats;
        gameObject.SetActive(true);


        characterPortrait.sprite = currentCharacter.sprite;

        healthText.text = currentCharacter.currentHealth + "/" + currentCharacter.maxHealth;
        manaText.text = currentCharacter.currentMana + "/" + currentCharacter.maxMana;
        staminaText.text = currentCharacter.currentStamina + "/" + currentCharacter.maxStamina;

        strengthText.text = currentCharacter.strength.ToString();
        technicalText.text = currentCharacter.technical.ToString();
        focusText.text = currentCharacter.focus.ToString();
        speedText.text = currentCharacter.speed.ToString();


    }





    public void OnSkillMenuButtonPressed()
    {
        if (!skillMenu.activeSelf)
        {
            weaponMenu.SetActive(false);
            itemMenu.SetActive(false);
            talismanMenu.SetActive(false);

            skillMenu.SetActive(true);
            OpenSkillsTab(currentCharacter);
        }
        else
        {
            skillMenu.SetActive(false);
        }
    }
    public void OnWeaponMenuButtonPressed()
    {
        if (!weaponMenu.activeSelf)
        {
            skillMenu.SetActive(false);
            itemMenu.SetActive(false);
            talismanMenu.SetActive(false);

            weaponMenu.SetActive(true);
            OpenWeaponsTab(currentCharacter);
        }
        else
        {
            weaponMenu.SetActive(false);
        }
    }
    public void OnItemMenuButtonPressed()
    {
        if (!itemMenu.activeSelf)
        {
            skillMenu.SetActive(false);
            weaponMenu.SetActive(false);
            talismanMenu.SetActive(false);

            itemMenu.SetActive(true);
            OpenItemsTab(currentCharacter);
        }
        else
        {
            itemMenu.SetActive(false);
        }
    }
    public void OnTalismanMenuButtonPressed()
    {
        if (!talismanMenu.activeSelf)
        {
            skillMenu.SetActive(false);
            weaponMenu.SetActive(false);
            itemMenu.SetActive(false);

            talismanMenu.SetActive(true);
            OpenTalismansTab(currentCharacter);
        }
        else
        {
            talismanMenu.SetActive(false);
        }
    }





    public void OpenSkillsTab(PersistanceStats stats)
    {

    }
    public void OpenWeaponsTab(PersistanceStats stats)
    {
        Debug.Log("OnItemToggleValueChanged");
        foreach (Transform child in weaponMenu.transform.GetChild(0)) Destroy(child.gameObject);


        foreach (Weapon weapon in InventoryManager.ownedWeapons)
        {
            if(weapon.type == currentCharacter.weaponType)
            {

                EquipableSelectionCard card = Instantiate(equipableCardPrefab, weaponMenu.transform.GetChild(0));


                card.image.sprite = weapon.sprite;


                if (InventoryManager.IsWeaponEquipped(weapon))
                {
                    card.button.interactable = false;
                }
                else
                {
                    card.button.onClick.RemoveAllListeners();
                    card.button.onClick.AddListener(() => InventoryManager.Equip(stats, weapon));
                    card.button.onClick.AddListener(() => weaponMenu.SetActive(false));
                }
            }
        }
    }
    public void OpenItemsTab(PersistanceStats stats)
    {
        foreach (Transform child in itemMenu.transform.GetChild(0)) Destroy(child.gameObject);


        foreach (Item item in InventoryManager.ownedItems)
        {

            EquipableSelectionCard card = Instantiate(equipableCardPrefab, itemMenu.transform.GetChild(0));

            card.image.sprite = item.sprite;
            if (InventoryManager.IsItemEquipped(item))
            {
                card.button.interactable = false;
            }
            else
            {
                card.button.onClick.RemoveAllListeners();
                card.button.onClick.AddListener(() => InventoryManager.Equip(stats, item));
                card.button.onClick.AddListener(() => itemMenu.SetActive(false));
            }


        }
    }
    public void OpenTalismansTab(PersistanceStats stats)
    {
        foreach (Transform child in talismanMenu.transform.GetChild(0)) Destroy(child.gameObject);


        foreach (Talisman talisman in InventoryManager.ownedTalismas)
        {
            EquipableSelectionCard card = Instantiate(equipableCardPrefab, talismanMenu.transform.GetChild(0));

            card.image.sprite = talisman.sprite;

            if (InventoryManager.IsTalismanEquipped(talisman))
            {
                card.button.interactable = false;
            }
            else
            {
                card.button.onClick.RemoveAllListeners();
                card.button.onClick.AddListener(() => InventoryManager.Equip(stats, talisman));
                card.button.onClick.AddListener(() => talismanMenu.SetActive(false));
            }

        }
    }








    public void MakeTalismanCard()
    {

    }
}
