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

        bool needTarget = skill.targetType == TargetingSystem.TargetType.ally
                       || skill.targetType == TargetingSystem.TargetType.enemy;
        if (needTarget)
        {
            TargetingSystem.StartTargeting(this, skill);
        }
        else if (skill.targetType == TargetingSystem.TargetType.all)
        {

        }
        else if (skill.targetType == TargetingSystem.TargetType.allEnemy)
        {

        }
        else if (skill.targetType == TargetingSystem.TargetType.allAlly)
        {

        }

    }
    //public override void SetTarget(Profile profile)
    //public override void LungeEnd()
}
