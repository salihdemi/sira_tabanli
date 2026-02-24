using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "Scriptable Objects/Useables/Skills/Burn")]
public class Burn : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.fire += 3;
    }
}
