using UnityEngine;

[CreateAssetMenu(fileName = "Slice", menuName = "Scriptable Objects/Skills/Slice")]
public class Slow : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        target.ChangeSpeed(-user.currentStrength);
        Debug.Log(user.name + " " + target.name + "'i " + name + " ile " + user.currentStrength + " yavaþlattý");
    }
}
