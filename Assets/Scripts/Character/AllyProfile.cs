using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{


    public override void LungeStart()
    {
        //Debug.Log(name + " hamlesini seçiyor");
        CharacterActionPanel.instance.OpenWriteThings(this);

    }
    public override void ChooseSkill(Skill skill)
    {
        currentSkill = skill;
        CharacterActionPanel.instance.CloseAndDisableAllPanels();

        bool needTargetýng = skill.targetType == TargetType.ally
                       || skill.targetType == TargetType.enemy;
        if (skill.targetType == TargetType.enemy)
        {
            if (FightManager.tauntedEnemy) SetTarget(FightManager.tauntedEnemy);
            else TargetingSystem.StartTargeting(this, skill);
        }
        else if (skill.targetType == TargetType.ally)
        {
            TargetingSystem.StartTargeting(this, skill);
        }
        else if (skill.targetType == TargetType.all)
        {
            SetTarget(null);
            lastTargetName = "Herkes";
        }
        else if (skill.targetType == TargetType.allEnemy)
        {
            SetTarget(null);
            lastTargetName = "Tüm düþmanlar";
        }
        else if (skill.targetType == TargetType.allAlly)
        {
            SetTarget(null);
            lastTargetName = "Tüm dostlar";
        }
        else if (skill.targetType == TargetType.self)
        {
            SetTarget(this);
            lastTargetName = name;
        }

    }
}
