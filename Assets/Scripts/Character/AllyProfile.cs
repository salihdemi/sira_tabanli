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
    public override void ChooseSkill(Useable useable)
    {
        currentUseable = useable;
        CharacterActionPanel.instance.CloseAndDisableAllPanels();

        bool needTargetýng = useable.targetType == TargetType.ally
                       || useable.targetType == TargetType.enemy;
        if (useable.targetType == TargetType.enemy)
        {
            if (FightManager.tauntedEnemy) SetTarget(FightManager.tauntedEnemy);
            else TargetingSystem.StartTargeting(this, useable);
        }
        else if (useable.targetType == TargetType.ally)
        {
            TargetingSystem.StartTargeting(this, useable);
        }
        else if (useable.targetType == TargetType.all)
        {
            SetTarget(null);
            lastTargetName = "Herkes";
        }
        else if (useable.targetType == TargetType.allEnemy)
        {
            SetTarget(null);
            lastTargetName = "Tüm düþmanlar";
        }
        else if (useable.targetType == TargetType.allAlly)
        {
            SetTarget(null);
            lastTargetName = "Tüm dostlar";
        }
        else if (useable.targetType == TargetType.self)
        {
            SetTarget(this);
            lastTargetName = name;
        }

    }
}
