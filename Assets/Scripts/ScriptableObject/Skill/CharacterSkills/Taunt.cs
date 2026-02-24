using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Taunt", menuName = "Scriptable Objects/Useables/Skills/Taunt")]
public class Taunt : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat
        target.Taunt();
    }

}
