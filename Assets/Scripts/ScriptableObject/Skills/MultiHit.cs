using System.Buffers;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiHit", menuName = "Scriptable Objects/Useables/Skills/MultiHit")]
public class MultiHit : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        user.StartCoroutine(MultiHitCoroutine(user, target));
    }
    private IEnumerator MultiHitCoroutine(Profile user, Profile target)
    {
        for (int i = 0; i < 10; i++)
        {
            target.AddToHealth(-user.currentTechnical, user);
            target.AddToHealth(-30, user);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
