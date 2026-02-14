using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CharacterActionPanel : MonoBehaviour
{
    public static CharacterActionPanel instance;


    

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private Transform panelsParent;

    [SerializeField] private Button attackButton;
    [SerializeField] private Button skillsButton;
    [SerializeField] private Button consumablesButton;
    [SerializeField] private Button itemButton;

    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject consumablesPanel;
    [SerializeField] private GameObject itemsPanel;

    [SerializeField] private Button buttonPrefab;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void OpenWriteThings(AllyProfile character)
    {
        gameObject.SetActive(true);

        WriteName(character);
        WriteAttack(character);
        WriteSkillsPanel(character);
        WriteConsumablesPanel(character);
        WriteItemButton(character);
    }

    public void CloseAndDisableAllPanels()
    {
        skillsPanel.SetActive(false);
        consumablesPanel.SetActive(false);
        itemsPanel.SetActive(false);

        gameObject.SetActive(false);
    }


    #region Write
    private void WriteName(AllyProfile character)
    {
        nameText.text = character.name;//!
    }
    private void WriteAttack(AllyProfile profile)
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => profile.ChooseSkill(profile.stats.attack));
    }


    //butonlarý her seferinde silip sýfýrdan oluþturuyor
    private void WriteSkillsPanel(AllyProfile profile)
    {
        if (skillsPanel == null) return;
        foreach (Transform child in skillsPanel.transform.GetChild(0))
        {
            Destroy(child.gameObject);//pool tipi yap!
        }
        // 2. Buton Oluþturma
        for (int i = 0; i < profile.stats.currentSkills.Count; i++)
        {
            Useable skill = profile.stats.currentSkills[i];

            Button button = MakeButton(skill, skillsPanel.transform, profile);

            button.GetComponent<Button>().onClick.AddListener(() => profile.ChooseSkill(skill));
        }

    }


    private void WriteConsumablesPanel(AllyProfile profile)
    {
        //Yemekleri yaz, !bu fonksiyona hiç gerek olmayadabilir


        if (consumablesPanel == null) return;
        foreach (Transform child in consumablesPanel.transform.GetChild(0)) Destroy(child.gameObject);//pool tipi yap!
        
        // 2. Buton Oluþturma
        foreach (Consumable consumable in InventoryManager.GetOwnedConsumable())
        {
            Debug.Log("buton eklendi");
            Button button = MakeButton(consumable, consumablesPanel.transform, profile);

            button.GetComponent<Button>().onClick.AddListener(() => InventoryManager.RemoveConsumable(consumable));
            button.GetComponent<Button>().onClick.AddListener(() => profile.ChooseSkill(consumable));
        }
    }
    private void WriteItemButton(AllyProfile profile)
    {
        itemButton.onClick.RemoveAllListeners();
        if (profile.stats.item == null)
        {
            itemButton.interactable = false;
        }
        else
        {
            itemButton.interactable = true;
            itemButton.onClick.AddListener(() => profile.ChooseSkill(profile.stats.item.skill));
        }
    }
    #endregion


    private Button MakeButton(Useable skill, Transform parent, AllyProfile profile)
    {
        Button button = Instantiate(buttonPrefab);
        TextMeshProUGUI text = button.transform.GetComponentInChildren<TextMeshProUGUI>();

        button.transform.SetParent(parent.GetChild(0), false);
        text.text = skill._name;

        if (profile.IsEnoughForSkill(skill)) button.interactable = true;
        else button.interactable = false;

        return button;
    }

}
