using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Skills/Block")]
public class Block : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        user.AddToHealth(user.GetPower());
        Debug.Log(user.name + " " + target.name + "'i " + name + " ile " + user.GetPower() + " iyileþtirdi");
    }
}
