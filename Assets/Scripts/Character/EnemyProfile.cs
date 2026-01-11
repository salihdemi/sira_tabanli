using UnityEngine;
using UnityEngine.UI;

public class EnemyProfile : Profile
{
    public override void TurnStart()
    {
        base.TurnStart();

        _Skill currentskill = BaseData.attack; //default hamle
        Debug.Log(currentskill);
        SetLunge(currentskill);
    }

    public override void TurnEnd()
    {
        base.TurnEnd();

        Debug.Log("turnend");
    }
    public override void SetLunge(_Skill skill)
    {
        Lunge = skill.Method;//secili saldýrýyý iþaretle

        Target = FightManager.instance.AllyProfiles[0];//default hedef!

        TurnEnd();
    }
    private void Update()
    {
        //Debug.Log(BaseData);
    }
}
