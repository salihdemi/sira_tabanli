using UnityEngine;
using UnityEngine.UI;

public class EnemyProfile : Profile
{
    public override void LungeStart()
    {
        //Debug.Log(name + " hamlesini seçiyor");

        _Skill currentskill = stats.attack; //default hamle
        ChooseSkill(currentskill);
    }
    public override void ChooseSkill(_Skill skill)
    {
        currentSkill = skill;
        SetTarget(FightManager.defaultTargetForEnemies);//!default hedef!

    }
    //public override void SetTarget(Profile profile)
    //public override void LungeEnd()
}
