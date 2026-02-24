using System.Collections;
using UnityEngine;

public abstract class ConsumableSkill : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;
    }
}
