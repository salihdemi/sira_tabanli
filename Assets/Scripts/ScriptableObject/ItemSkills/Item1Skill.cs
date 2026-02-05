
using UnityEngine;

[CreateAssetMenu(fileName = "Item1Skill", menuName = "Scriptable Objects/Useables/ItemSkills/Item1Skill")]
public class Item1Skill : ItemSkill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        target.AddToHealth(-user.currentStrength);
        Debug.Log("item kullanýldý");
    }
}
