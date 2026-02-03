using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Scriptable Objects/Toys/Bomb")]
public class Bomb : Toy
{
    [SerializeField] float damage = 10;
    public override void Method(Profile user, Profile target)
    {
        base.Method(user, target);//decrease calistiriyor mu?!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        target.AddToHealth(-damage);
        //Debug.Log(user.name + " " + target.name + "'a " + name + " ile " + user.GetPower() + " hasar verdi");
    }
}
