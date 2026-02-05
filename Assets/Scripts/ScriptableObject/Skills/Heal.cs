using UnityEngine;

[CreateAssetMenu(fileName = "Burst", menuName = "Scriptable Objects/Useables/Skills/Heal")]
public class Heal : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.AddToHealth(target.currentStrength * 3);
        Debug.Log(user.name + " " + target.name + "'i " + name + " ile " + user.currentStrength * 3 + " iyileþtirdi");
    }
}
