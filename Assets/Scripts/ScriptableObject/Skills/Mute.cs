using UnityEngine;

[CreateAssetMenu(fileName = "Mute", menuName = "Scriptable Objects/Useables/Skills/Mute")]
public class Mute : Skill
{
    public override void Method(Profile user, Profile target)
    {

        target.mute++;
    }
}
