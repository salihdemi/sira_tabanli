using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "Scriptable Objects/Skills/CharacterSkills/Burn")]
public class Burn : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.Burn(3);
        yield return null;//!
    }
}
