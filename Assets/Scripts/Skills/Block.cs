using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Skills/Block")]
public class Block : Useable
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
