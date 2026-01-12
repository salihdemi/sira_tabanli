using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{

    public override void LungeStart()
    {
        Debug.Log(name + " hamlesini seçiyor");

        CharacterActionPanel.instance.WriteThings(this);

        CharacterActionPanel.instance.gameObject.SetActive(true);

    }
    public override void ChooseSkill(_Skill skill)
    {
        currentSkill = skill;
        CharacterActionPanel.instance.DisableAllPanels();

        //hedef seçecek
        TargetingSystem.instance.StartTargeting(this, skill);

        CharacterActionPanel.instance.gameObject.SetActive(false);
    }
    public override void SetTarget(Profile profile)
    {
        target = profile;

        LungeEnd();//!
    }
    public override void LungeEnd()
    {

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






        FightManager.instance.turnScheduler.CheckNextCharacter();

    }
}
