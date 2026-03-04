using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aggressive Behaviour", menuName = "AI/Behaviours/Aggressive")]
public class AggressiveEnemyBehaviour : BehaviourSet
{
    //Random bir  skilli en düþük canlý düþman ya da dosta uygular
    public override void DecideLunge(ProfileLungeHandler lungeHandler)
    {

        Skill selectedSkill = lungeHandler.profile.stats.attack; // Varsayýlan olarak normal saldýrý


        List<Skill> usableSkills = lungeHandler.profile.stats.currentSkills.ToList();

        if (usableSkills.Count > 0)
        {
            selectedSkill = usableSkills[0];//defaultSkill!
            //selectedSkill = GetRandomUsableSkill(lungeHandler.profile);
        }
        ChooseSkill(lungeHandler, selectedSkill);









        Profile target = null;

        switch (selectedSkill.targetType)
        {
            case TargetType.enemy:
                //support
                target = GetLowestHealthEnemy();
                break;

            case TargetType.ally:
                //attack
                target = GetLowestHealthAlly();
                break;

            case TargetType.self:
                target = lungeHandler.profile;
                break;
        }
        ChooseTarget(lungeHandler, target);
    }


}