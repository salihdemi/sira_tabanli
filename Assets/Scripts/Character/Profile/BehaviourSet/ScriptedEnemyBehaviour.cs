using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

[CreateAssetMenu(fileName = "Scripted Enemy Behaviour", menuName = "AI/Behaviours/Aggressive")]
public class ScriptedEnemyBehaviour : EnemyBehaviourSet
{
    [SerializeField] private Skill[] skillArray;
    private int skillIndex;


    //Random bir  skilli en düþük canlý düþman ya da dosta uygular
    public override void DecideLunge(ProfileLungeHandler lungeHandler)
    {
        skillArray = lungeHandler.profile.stats.currentSkills.Append(lungeHandler.profile.stats.attack).ToArray();

        Skill selectedSkill = skillArray[skillIndex];

        skillIndex ++;

        ChooseSkill(lungeHandler, selectedSkill);







        Profile target = ReturnTargetByTargetType(selectedSkill.targetType, lungeHandler.profile);
        ChooseTarget(lungeHandler, target);
    }


}