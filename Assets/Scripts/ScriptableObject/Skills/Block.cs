using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Useables/Skills/Block")]
public class Block : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        user.AddToHealth(user.currentStrength, user);
        Debug.Log(user.name + " " + user.name + "'i " + name + " ile " + user.currentStrength + " iyileþtirdi");
    }
}
