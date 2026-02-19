using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Useables/Attack")]
public class Attack : Useable
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat


        string text = user.name + " " + name + " " + target.name;
        ConsolePanel.instance.WriteConsole(text);


        //saldýrýyý yap
        target.AddToHealth(-user.currentStrength, user);
        Debug.Log(user.name + " " + target.name + "'a " + name + " ile " + user.currentStrength + " hasar verdi");


    }
}
