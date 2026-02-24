using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Useables/Skills/Block")]
public class Block : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        user.AddToHealth(user.currentStrength, user);
        Debug.Log(user.name + " " + user.name + "'i " + name + " ile " + user.currentStrength + " iyileþtirdi");
    }
}
