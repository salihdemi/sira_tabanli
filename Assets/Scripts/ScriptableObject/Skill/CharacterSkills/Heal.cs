using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Burst", menuName = "Scriptable Objects/Skills/CharacterSkills/Heal")]
public class Heal : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.AddToHealth(target.currentStrength * 3, user);
        Debug.Log(user.name + " " + target.name + "'i " + name + " ile " + user.currentStrength * 3 + " iyileþtirdi");
        yield return null;//!
    }
}
