using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "StaminaSkill", menuName = "Scriptable Objects/Useables/Skills/StaminaSkill")]
public class StaminaSkill : CharacterSkill
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
