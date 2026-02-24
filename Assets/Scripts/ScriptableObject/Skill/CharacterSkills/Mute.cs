using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Mute", menuName = "Scriptable Objects/Useables/Skills/Mute")]
public class Mute : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        target.mute++;
    }
}
