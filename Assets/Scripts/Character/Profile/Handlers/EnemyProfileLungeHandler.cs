using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyProfileLungeHandler : ProfileLungeHandler
{


    public override void LungeStart()
    {
        profile.stats.behaviourSet.DecideLunge(profile.lungeHandler);
    }
    public override void ChooseSkill(Skill skill)
    {
        if (profile.IsEnoughForSkill(skill)) currentSkill = skill;
        else currentSkill = null;//her çaðýrmada nullcheck yapýlmalý!
    }
    public override void ChooseTarget(Profile target)
    {
        if (target == null)//Cok hedefli skillerde
        {
            FinishLunge();
            return;
        }
        currentTarget = target;

        FinishLunge();//!
    }
    //public override void LungeEnd()

}
