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
    public override void ChooseSkill(Useable skill)
    {
        currentSkill = skill;
        CharacterActionPanel.instance.CloseAndDisableAllPanels();

        bool needTarget = skill.targetType == TargetType.ally
                       || skill.targetType == TargetType.enemy;
        if (needTarget)
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
            SetTarget(null);
            lastTargetName = name;
        }

    }
}
