using UnityEngine;
using UnityEngine.UI;

public class EnemyProfile : Profile
{
    public override void LungeStart()
    {
        //Debug.Log(name + " hamlesini seçiyor");

        Useable currentskill = stats.attack; //default hamle
        ChooseSkill(currentskill);
    }
    public override void ChooseSkill(Useable skill)
    {
        currentUseable = skill;
        Debug.Log(FightManager.tauntedAlly + " tauntluaaaaaaaaaaaaaaaaaaaaaaaaaa");
        if (FightManager.tauntedAlly)
        {
            Debug.Log(FightManager.tauntedAlly + " tauntlu");
            SetTarget(FightManager.tauntedAlly);
        }
        else
        {
            SetTarget(FightManager.defaultTargetForEnemies);//!default hedef!
        }

    }
    //public override void SetTarget(Profile profile)
    //public override void LungeEnd()
}
