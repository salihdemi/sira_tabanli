using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Skills/Block")]
public class Block : Skill
{
    Block()
    {
        targetType = TargetingSystem.TargetType.self;
    }
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        user.AddToHealth(user.currentStrength);
        Debug.Log(user.name + " " + user.name + "'i " + name + " ile " + user.currentStrength + " iyileþtirdi");
    }
}
