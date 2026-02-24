using System.Collections;
using UnityEngine;

public class CharacterSkill : Skill
{
    [Header("Need")]
    public float healthCost;
    public float staminaCost;
    public float manaCost;
    protected void Consume(Profile user)
    {
        user.AddToHealth(-healthCost, user);
        user.AddToMana(-manaCost);
        user.AddToStamina(-staminaCost);
    }//!


    [Header("Skill")]
    public Sprite sprite;
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;
    }
}
public enum TargetType
{
    enemy,
    ally,

    self,
    allEnemy,
    allAlly,
    all
}
