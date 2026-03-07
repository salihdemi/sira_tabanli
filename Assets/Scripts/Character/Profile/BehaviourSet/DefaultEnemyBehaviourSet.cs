using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "DefaultBehaviour", menuName = "AI/Behaviours/Default")]
public class DefaultEnemyBehaviourSet : EnemyBehaviourSet
{
    public override void DecideLunge(ProfileLungeHandler lungeHandler)
    {
        Skill skill = GetRandomUsableSkill(lungeHandler.profile);
        ChooseSkill(lungeHandler, skill);

        Profile target = ReturnTargetByTargetType(skill.targetType, lungeHandler.profile);
        ChooseTarget(lungeHandler, target);

    }

}