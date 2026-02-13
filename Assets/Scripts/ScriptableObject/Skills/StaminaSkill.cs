using UnityEngine;

[CreateAssetMenu(fileName = "StaminaSkill", menuName = "Scriptable Objects/Useables/Skills/StaminaSkill")]
public class StaminaSkill : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.AddToHealth(user.currentFocus, user);
    }
}
