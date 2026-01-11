using UnityEngine;
using UnityEngine.UI;

public class EnemyProfile : Profile
{
    public override void TurnStart()
    {
        base.TurnStart();

        _Skill currentskill = BaseData.attack; //default hamle
        ChooseSkill(currentskill);
    }
    public override void ChooseSkill(_Skill skill)
    {

        target = FightManager.instance.AllyProfiles[0];//default hedef!

        TurnEnd();
    }
    public override void SetTarget(Profile profile)
    {
        target = profile;

        TurnEnd();//!
    }
    public override void TurnEnd()
    {
        base.TurnEnd();

        Debug.Log("turnend");
    }
}
