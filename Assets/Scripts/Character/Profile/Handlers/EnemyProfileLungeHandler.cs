using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyProfileLungeHandler : ProfileLungeHandler
{


    public override void LungeStart()
    {
        ChooseSkill(profile.stats.behaviourSet.DecideSkill(this));
    }
    public override void ChooseSkill(Skill skill)
    {
        currentSkill = skill;
        Profile target;

        // Sadece saldýrý skillerinde Taunt kontrolü yapmalýsýn!
        if (skill.targetType == TargetType.enemy && FightManager.tauntedAlly != null)
        {
            target = FightManager.tauntedAlly;
        }
        else
        {
            target = profile.stats.behaviourSet.DecideTarget(this, skill.targetType);
        }

        SetTarget(target);
    }
    public override void SetTarget(Profile profile)
    {
        if (profile == null)//Cok hedefli skillerde
        {
            FinishLunge();
            return;
        }
        currentTarget = profile;
        //lastTargetName = currentTarget.name;

        FinishLunge();//!
    }
    //public override void LungeEnd()

}
