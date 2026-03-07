using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfileLungeHandler : ProfileLungeHandler
{


    public override void LungeStart()
    {
        CharacterActionPanel.instance.OpenWriteThings(profile);

    }
    public override void ChooseSkill(Skill skill)
    {
        currentSkill = skill;
        CharacterActionPanel.instance.CloseAndDisableAllPanels();


        switch (skill.targetType)
        {
            case TargetType.enemy:
                if (FightManager.tauntedEnemy) ChooseTarget(FightManager.tauntedEnemy);
                else TargetingSystem.StartTargeting(profile, skill);
                break;

            case TargetType.ally:
                TargetingSystem.StartTargeting(profile, skill);
                break;

            case TargetType.self:
                ChooseTarget(profile);
                break;

            default:
                ChooseTarget(null);
                break;
        }
    }

    public override void ChooseTarget(Profile profile)
    {

        if (profile == null)//Cok hedefli skillerde
        {
            FinishLunge();
            return;
        }
        currentTarget = profile;
        //lastTargetName = currentTarget.name;

        FinishLunge();//!
    }
}
