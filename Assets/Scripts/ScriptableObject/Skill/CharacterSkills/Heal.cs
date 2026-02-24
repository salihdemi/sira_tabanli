using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Burst", menuName = "Scriptable Objects/Useables/Skills/Heal")]
public class Heal : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.AddToHealth(target.currentStrength * 3, user);
        Debug.Log(user.name + " " + target.name + "'i " + name + " ile " + user.currentStrength * 3 + " iyileþtirdi");
    }
}
