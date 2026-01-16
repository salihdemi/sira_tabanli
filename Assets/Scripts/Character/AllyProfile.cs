using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{
    public static event Action<AllyProfile> OnAnyAllyLungeStart;
    public static event Action<Profile, _Skill> OnAnyAllyChoseSkill;

    public override void LungeStart()
    {
        Debug.Log(name + " hamlesini seçiyor");

        OnAnyAllyLungeStart.Invoke(this);

    }
    public override void ChooseSkill(_Skill skill)
    {
        Debug.Log("settarget");
        currentSkill = skill;
        OnAnyAllyChoseSkill?.Invoke(this, skill);

    }
    //public override void SetTarget(Profile profile)
    //public override void LungeEnd()
}
