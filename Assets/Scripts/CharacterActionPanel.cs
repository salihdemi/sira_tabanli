using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionPanel : MonoBehaviour
{
    public static CharacterActionPanel instance;

    private TextMeshProUGUI nameText;
    private Transform buttonsParent;
    private Transform panelsParent;

    private Button attackButton;
    private Button skillsButton;
    private Button foodsButton;
    private Button toysButton;

    private GameObject skillsPanel;
    private GameObject foodsPanel;
    private GameObject toysPanel;

    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Birden fazla CharacterActionPanel var");
            Debug.Log(this.name);
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        CheckInstance();

        FindFirstChilds();
        FindButtons();
        FindPanels();

        AssignButtons();
    }

    #region Finds-Assigns
    private void FindFirstChilds()
    {
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttonsParent = transform.GetChild(1);
        panelsParent = transform.GetChild(2);
    }
    private void FindButtons()
    {
        attackButton = buttonsParent.GetChild(0).GetComponent<Button>();
        skillsButton = buttonsParent.GetChild(1).GetComponent<Button>();
        foodsButton = buttonsParent.GetChild(2).GetComponent<Button>();
        toysButton = buttonsParent.GetChild(3).GetComponent<Button>();
    }
    private void FindPanels()
    {
        skillsPanel = panelsParent.GetChild(0).gameObject;
        foodsPanel = panelsParent.GetChild(1).gameObject;
        toysPanel = panelsParent.GetChild(2).gameObject;
    }
    private void AssignButtons()
    {
        skillsButton.onClick.AddListener(() => skillsPanel.SetActive(true));
        foodsButton.onClick.AddListener(() => foodsPanel.SetActive(true));
        toysButton.onClick.AddListener(() => toysPanel.SetActive(true));
    }
    #endregion


    public void DisableAllPanels()
    {
        skillsPanel.SetActive(false);
        foodsPanel.SetActive(false);
        toysPanel.SetActive(false);
    }

    public void WriteThings(AllyProfile character)
    {
        WriteName(character);
        WriteAttack(character);
        WriteSkillsPanel(character);
        WriteFoodsPanel();
        WriteToysPanel();
    }


    #region Write
    private void WriteName(AllyProfile character)
    {
        nameText.text = character.name;
    }
    private void WriteAttack(AllyProfile profile)
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => profile.SetLunge(profile.BaseData.attack));
    }
    private void WriteSkillsPanel(AllyProfile profile)
    {
        foreach (Transform child in skillsPanel.transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }

        foreach (_Skill skill in profile.BaseData.skills)
        {
            skill.AddButton(profile, skillsPanel);
        }
    }
    private void WriteFoodsPanel()
    {
        //Yemekleri yaz, !bu fonksiyona hiç gerek olmayadabilir
    }
    private void WriteToysPanel()
    {
        //Oyuncaklarý yaz, !bu fonksiyona hiç gerek olmayadabilir
    }
    #endregion
}
