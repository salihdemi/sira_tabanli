using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Mute", menuName = "Scriptable Objects/Skills/CharacterSkills/Mute")]
public class Mute : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        target.Mute(1);
        yield return null;//!
    }
}
