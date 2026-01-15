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

    }
    public override void ChooseSkill(_Skill skill)
    {
        currentSkill = skill;
        CharacterActionPanel.instance.DisableAllPanels();

        //hedef seçecek
        TargetingSystem.instance.StartTargeting(this, skill);

    }
    public override void SetTarget(Profile profile)
    {
        currentTarget = profile;

        LungeEnd();//!
    }
    public override void LungeEnd()
    {

        //burayý al
        foreach (Profile profile in FightManager.instance.EnemyProfiles)
        {
            profile.view.SetSelectable(false);
        }

        foreach (Profile profile in FightManager.instance.AllyProfiles)
        {
            profile.view.SetSelectable(false);
        }






        FightManager.instance.turnScheduler.CheckNextCharacter();

    }
}
