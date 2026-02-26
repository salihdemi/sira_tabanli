using UnityEngine;

public class FireEnemy : Profile
{
    public override void AddToHealth(float amount, Profile dealer)
    {
        Debug.Log("fire");
        if (fire > 0)
        {
            base.AddToHealth(amount, dealer);
        }
    }
}
