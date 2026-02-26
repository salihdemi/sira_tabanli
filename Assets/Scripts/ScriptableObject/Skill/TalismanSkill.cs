using System.Collections;
using UnityEngine;

public class TalismanSkill : Skill
{
    public virtual IEnumerator Method(Profile user, Profile target, float damage)
    {
        yield return new WaitForSeconds(1);
    }
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;
    }
}
