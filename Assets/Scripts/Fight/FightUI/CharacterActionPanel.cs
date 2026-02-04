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
    [SerializeField] private Button foodsButton;
    [SerializeField] private Button toysButton;

    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject foodsPanel;
    [SerializeField] private GameObject toysPanel;

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
        WriteFoodsPanel(character);
        WriteToysPanel(character);
    }

    public void CloseAndDisableAllPanels()
    {
        skillsPanel.SetActive(false);
        foodsPanel.SetActive(false);
        toysPanel.SetActive(false);

        gameObject.SetActive(false);
    }


    #region Write
    private void WriteName(AllyProfile character)
    {
        nameText.text = character.name;
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
        for (int i = 0; i < profile.stats.skills.Count; i++)
        {
            Useable skill = profile.stats.skills[i];

            Button button = MakeButton(skill, skillsPanel.transform);

            button.GetComponent<Button>().onClick.AddListener(() => profile.ChooseSkill(skill));
        }

    }


    private void WriteFoodsPanel(AllyProfile profile)
    {
        //Yemekleri yaz, !bu fonksiyona hiç gerek olmayadabilir


        if (foodsPanel == null) return;
        foreach (Transform child in foodsPanel.transform.GetChild(0))
        {
            Destroy(child.gameObject);//pool tipi yap!
        }
        // 2. Buton Oluþturma
        foreach (Food food in InventoryManager.GetOwnedFoods())
        {
            Button button = MakeButton(food, foodsPanel.transform);

            button.GetComponent<Button>().onClick.AddListener(() => profile.ChooseSkill(food));
        }
    }
    private void WriteToysPanel(AllyProfile profile)
    {
        //Oyuncaklarý yaz, !bu fonksiyona hiç gerek olmayadabilir

        if (toysPanel == null) return;
        foreach (Transform child in toysPanel.transform.GetChild(0))
        {
            Destroy(child.gameObject);//pool tipi yap!
        }
        // 2. Buton Oluþturma
        foreach (Toy toy in InventoryManager.GetOwnedToys())
        {
            Button button = MakeButton(toy, toysPanel.transform);

            button.GetComponent<Button>().onClick.AddListener(() => profile.ChooseSkill(toy));
        }
    }
    #endregion


    private Button MakeButton(Useable skill, Transform parent)
    {
        Button button = Instantiate(buttonPrefab);
        TextMeshProUGUI text = button.transform.GetComponentInChildren<TextMeshProUGUI>();

        button.transform.SetParent(parent.GetChild(0), false);
        text.text = skill.name;



        return button;
    }
}
