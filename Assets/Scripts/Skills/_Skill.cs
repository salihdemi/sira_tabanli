using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class _Skill : ScriptableObject
{
    //private AnimationClip clip;
    //private AudioClip clip;
    public string _name;
    public TargetingSystem.TargetType targetType = TargetingSystem.TargetType.enemy;
    public abstract void Method(Profile user, Profile target);

    public void AddButton(AllyProfile character, GameObject skillsPanel)
    {
        //Buton
        GameObject newSkillButton = new GameObject(name + "_Button");
        newSkillButton.transform.parent = skillsPanel.transform.GetChild(0);//parent
        newSkillButton.AddComponent<CanvasRenderer>();
        newSkillButton.AddComponent<Image>();
        Button button = newSkillButton.AddComponent<Button>();


        //Text
        GameObject ButtonText = new GameObject("text");
        ButtonText.transform.parent = newSkillButton.transform;
        TextMeshProUGUI text = ButtonText.AddComponent<TextMeshProUGUI>();
        text.text = name;
        text.color = Color.black;
        //text.fontSize = 12;


        //Buton event
        button.onClick.AddListener(() => character.SetLunge(this));
    }
}
