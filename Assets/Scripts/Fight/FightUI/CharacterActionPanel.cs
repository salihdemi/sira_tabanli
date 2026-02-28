using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionPanel : MonoBehaviour
{
    public static CharacterActionPanel instance;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button itemButton;

    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject itemsPanel;
    [SerializeField] private GameObject consumablesPanel;

    [SerializeField] private Button buttonPrefab;
    
    

    // --- POOL LÝSTESÝ ---
    private List<Button> _buttonPool = new List<Button>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void OpenWriteThings(Profile character)
    {
        gameObject.SetActive(true);
        WriteName(character);
        WriteAttack(character);
        WriteSkillsPanel(character);
        WriteConsumablesPanel(character);
        WriteItemButton(character);
    }

    // --- HAVUZ TEMÝZLEME (Destroy yerine Pasife Çekme) ---
    private void ClearButtons(Transform container)
    {
        foreach (Transform child in container)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void WriteSkillsPanel(Profile profile)
    {
        if (skillsPanel == null) return;
        Transform container = skillsPanel.transform.GetChild(0);

        ClearButtons(container); // ESKÝ: Destroy(child.gameObject)

        for (int i = 0; i < profile.stats.currentSkills.Count; i++)
        {
            Skill skill = profile.stats.currentSkills[i];
            Button button = GetButtonFromPool(container);

            button.GetComponentInChildren<TextMeshProUGUI>().text = skill._name;
            button.interactable = profile.IsEnoughForSkill(skill);

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => profile.lungeHandler.ChooseSkill(skill));
        }
    }

    private void WriteConsumablesPanel(Profile profile)
    {
        if (consumablesPanel == null) return;
        Transform container = consumablesPanel.transform.GetChild(0);

        ClearButtons(container); // ESKÝ: Destroy(child.gameObject)

        List<Consumable> ownedConsumables = InventoryManager.GetOwnedConsumable();
        foreach (Consumable consumable in ownedConsumables)
        {
            Button button = GetButtonFromPool(container);

            button.GetComponentInChildren<TextMeshProUGUI>().text = consumable._name;
            button.interactable = true;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => InventoryManager.RemoveConsumable(consumable));
            button.onClick.AddListener(() => profile.lungeHandler.ChooseSkill(consumable.skill));
            // Eþya bitince paneli yenilemek isteyebilirsin:
            button.onClick.AddListener(() => WriteConsumablesPanel(profile));
        }
    }

    // --- GENERIC POOL METHOD ---
    private Button GetButtonFromPool(Transform parent)
    {
        // Havuzda boþta (inactive) buton var mý bak
        for (int i = 0; i < _buttonPool.Count; i++)
        {
            if (!_buttonPool[i].gameObject.activeSelf)
            {
                _buttonPool[i].transform.SetParent(parent, false);
                _buttonPool[i].gameObject.SetActive(true);
                return _buttonPool[i];
            }
        }

        // Yoksa yeni oluþtur
        Button newButton = Instantiate(buttonPrefab, parent);
        _buttonPool.Add(newButton);
        return newButton;
    }

    // Diðer Write metodlarýn (WriteName, WriteAttack, WriteItemButton) ayný kalabilir...
    private void WriteName(Profile character) => nameText.text = character.stats._name;

    private void WriteAttack(Profile profile)
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => profile.lungeHandler.ChooseSkill(profile.stats.attack));
    }

    private void WriteItemButton(Profile profile)
    {
        itemButton.onClick.RemoveAllListeners();
        if (profile.stats.item == null)
        {
            itemButton.interactable = false;
        }
        else
        {
            itemButton.interactable = true;
            itemButton.onClick.AddListener(() => profile.lungeHandler.ChooseSkill(profile.stats.item.skill));
        }
    }

    public void CloseAndDisableAllPanels()
    {
        skillsPanel.SetActive(false);
        consumablesPanel.SetActive(false);
        if (itemsPanel != null) itemsPanel.SetActive(false); // Eksik null check eklendi
        gameObject.SetActive(false);
    }



}