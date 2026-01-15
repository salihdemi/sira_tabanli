using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{
    public static event Action<AllyProfile> OnLungeStart;
    public static event Action<Profile, _Skill> OnSkillChosen;

    Profile a;
    public override void LungeStart()
    {
        Debug.Log(name + " hamlesini seçiyor");

        OnLungeStart.Invoke(this);

    }
    public override void ChooseSkill(_Skill skill)
    {
        currentSkill = skill;
        OnSkillChosen?.Invoke(this, skill);

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
