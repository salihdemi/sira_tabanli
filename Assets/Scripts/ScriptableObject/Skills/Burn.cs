using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "Scriptable Objects/Useables/Skills/Burn")]
public class Burn : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.fire += 3;
    }
}
