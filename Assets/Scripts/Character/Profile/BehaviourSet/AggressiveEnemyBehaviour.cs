using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aggressive Behaviour", menuName = "AI/Behaviours/Aggressive")]
public class AggressiveEnemyBehaviour : EnemyBehaviourSet
{
    public override Skill DecideSkill(EnemyProfileLungeHandler handler)
    {
        Profile self = handler.profile;
        Skill selectedSkill = self.stats.attack; // Varsayýlan olarak normal saldýrý


        List<Skill> usableSkills = self.stats.currentSkills.ToList();

        if (usableSkills.Count > 0)
        {
            selectedSkill = usableSkills[0];//defaultSkill
        }
        return selectedSkill;
    }

    public override Profile DecideTarget(EnemyProfileLungeHandler lungeHandler, TargetType type)
    {
        Profile target = null;

        if (type == TargetType.enemy)
        {
            //support
            target = GetLowestHealthEnemy();
        }
        else if (type == TargetType.ally)
        {
            //attack
            target = GetLowestHealthAlly();
        }
        else if (type == TargetType.self)
        {
            target = lungeHandler.profile;
        }
        
        return target;
    }
}