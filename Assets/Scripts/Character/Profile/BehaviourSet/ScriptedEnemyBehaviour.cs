using System.Collections.Generic;
using UnityEngine;

public enum ScriptedBehaviourEndMode { Loop, RepeatLast }

[CreateAssetMenu(fileName = "ScriptedBehaviour", menuName = "AI/Behaviours/Scripted")]
public class ScriptedEnemyBehaviour : EnemyBehaviourSet
{
    [SerializeField] private List<Skill> skillSequence = new List<Skill>();
    [SerializeField] private ScriptedBehaviourEndMode endMode;
    private int skillIndex;

    public override void DecideLunge(ProfileLungeHandler lungeHandler)
    {
        if (skillSequence == null || skillSequence.Count == 0)
        {
            ChooseSkill(lungeHandler, lungeHandler.profile.stats.attack);
            ChooseTarget(lungeHandler, GetRandomAlly());
            return;
        }

        Skill selectedSkill = skillSequence[skillIndex];
        skillIndex++;

        if (skillIndex >= skillSequence.Count)
            skillIndex = endMode == ScriptedBehaviourEndMode.Loop ? 0 : skillSequence.Count - 1;

        ChooseSkill(lungeHandler, selectedSkill);
        ChooseTarget(lungeHandler, ReturnTargetByTargetType(selectedSkill.targetType, lungeHandler.profile));
    }
}
