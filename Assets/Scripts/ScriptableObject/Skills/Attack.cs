using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Useables/Skills/Attack")]
public class Attack : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        target.AddToHealth(-user.currentStrength);
        Debug.Log(user.name + " " + target.name + "'a " + name + " ile " + user.currentStrength + " hasar verdi");
    }
}
