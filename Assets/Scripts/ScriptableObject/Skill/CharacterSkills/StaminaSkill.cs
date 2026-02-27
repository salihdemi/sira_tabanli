using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "StaminaSkill", menuName = "Scriptable Objects/Skills/CharacterSkills/StaminaSkill")]
public class StaminaSkill : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.AddToHealth(user.currentFocus, user);
    }
}
