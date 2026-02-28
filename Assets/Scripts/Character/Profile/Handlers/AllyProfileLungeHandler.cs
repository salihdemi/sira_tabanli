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
        Debug.Log(name + " hamlesini seçiyor");
        CharacterActionPanel.instance.OpenWriteThings(profile);

    }
    public override void ChooseSkill(Skill skill)
    {
        Debug.Log(profile.stats._name + skill);
        currentSkill = skill;
        CharacterActionPanel.instance.CloseAndDisableAllPanels();

        bool needTargetýng = skill.targetType == TargetType.ally
                       || skill.targetType == TargetType.enemy;
        if (skill.targetType == TargetType.enemy)
        {
            if (FightManager.tauntedEnemy) SetTarget(FightManager.tauntedEnemy);
            else TargetingSystem.StartTargeting(profile, skill);
        }
        else if (skill.targetType == TargetType.ally)
        {
            TargetingSystem.StartTargeting(profile, skill);
        }
        else if (skill.targetType == TargetType.self)
        {
            SetTarget(profile);
        }
        //gerek var mý?
        else if (skill.targetType == TargetType.all)
        {
            SetTarget(null);
        }
        else if (skill.targetType == TargetType.allEnemy)
        {
            SetTarget(null);
        }
        else if (skill.targetType == TargetType.allAlly)
        {
            SetTarget(null);
        }

    }
}
