using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Ally", menuName = "Scriptable Objects/Characters/Ally")]
public class Ally : CharacterBase
{


    [HideInInspector] public AllyProfile characterAlly;


    public override void MakeProfile()
    {
        profile = FightManager.instance.MakeAllyProfile();
        profile.characterData = this;
        profile.gameObject.name = name;
    }

    public void LearnSkill(_Skill skill)
    {
        Debug.Log(skill.ToString());
        if(skills.Contains(skill))
        {
            Debug.Log("bu skill zaten öðrenilmiþ");
            return;
        }
        Debug.Log("Skill öðrenldi");
        skills.Add(skill);
    }

}
