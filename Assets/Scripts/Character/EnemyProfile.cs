using UnityEngine;
using UnityEngine.UI;

public class EnemyProfile : Profile
{
    public override void TurnStart()
    {
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
        Debug.Log("turnend");
    }
}
