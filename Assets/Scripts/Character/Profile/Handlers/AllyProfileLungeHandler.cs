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

        bool needTargetýng = skill.targetType == TargetType.ally
                       || skill.targetType == TargetType.enemy;
        if (skill.targetType == TargetType.enemy)
        {
            if (FightManager.tauntedEnemy) ChooseTarget(FightManager.tauntedEnemy);
            else TargetingSystem.StartTargeting(profile, skill);
        }
        else if (skill.targetType == TargetType.ally)
        {
            TargetingSystem.StartTargeting(profile, skill);
        }
        else if (skill.targetType == TargetType.self)
        {
            ChooseTarget(profile);
        }
        //gerek var mý?
        else if (skill.targetType == TargetType.all)
        {
            ChooseTarget(null);
        }
        else if (skill.targetType == TargetType.allEnemy)
        {
            ChooseTarget(null);
        }
        else if (skill.targetType == TargetType.allAlly)
        {
            ChooseTarget(null);
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
