using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "DefaultBehaviour", menuName = "AI/Behaviours/Default")]
public class DefaultEnemyBehaviourSet : EnemyBehaviourSet
{
    public override void DecideLunge(ProfileLungeHandler lungeHandler)
    {
        ChooseSkill(lungeHandler, GetRandomUsableSkill(lungeHandler.profile));
        ChooseTarget(lungeHandler, GetRandomAlly());
    }
}