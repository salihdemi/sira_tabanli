using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{

    public override void TurnStart()
    {
        CharacterActionPanel.instance.WriteThings(this);

        CharacterActionPanel.instance.gameObject.SetActive(true);

    }
    public override void ChooseSkill(_Skill skill)
    {

        CharacterActionPanel.instance.DisableAllPanels();

        //hedef seçecek
        TargetingSystem.instance.StartTargeting(this, skill);
    }
    public override void SetTarget(Profile profile)
    {
        target = profile;

        TurnEnd();//!
    }
    public override void TurnEnd()
    {
        CharacterActionPanel.instance.gameObject.SetActive(false);

        //burayý al
        foreach (Profile profile in FightManager.instance.EnemyProfiles)
        {
            profile.button.interactable = false;
            profile.button.onClick.RemoveAllListeners();
        }

        foreach (Profile profile in FightManager.instance.AllyProfiles)
        {
            profile.button.interactable = false;
            profile.button.onClick.RemoveAllListeners();
        }





        CharacterActionPanel.instance.gameObject.SetActive(false);
    }
}
