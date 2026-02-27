
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Item1Skill", menuName = "Scriptable Objects/Skills/ItemSkills/Item1Skill")]
public class Item1Skill : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        target.AddToHealth(-user.currentStrength, user);
        Debug.Log("item kullanýldý");
    }
}
