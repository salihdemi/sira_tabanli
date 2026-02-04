using UnityEngine;

[CreateAssetMenu(fileName = "ChargedAttack", menuName = "Scriptable Objects/Skills/ChargedAttack")]
public class ProcWeakness : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        target.ChangeStrength(-5);
        Debug.Log(user.name + " " + target.name + "'a " + name + " ile " + 5 + " zayýflattý");
    }
}
