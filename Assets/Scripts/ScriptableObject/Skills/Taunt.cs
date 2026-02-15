using UnityEngine;

[CreateAssetMenu(fileName = "Taunt", menuName = "Scriptable Objects/Useables/Skills/Taunt")]
public class Taunt : Skill
{
    public override void Method(Profile user, Profile target)
    {
        //animasyonu oynat
        //sesi oynat
        target.Taunt();
    }

}
