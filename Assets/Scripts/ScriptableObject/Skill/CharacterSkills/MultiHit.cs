using System.Buffers;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiHit", menuName = "Scriptable Objects/Skills/CharacterSkills/MultiHit")]
public class MultiHit : Skill
{
    [Header("MultiHit")]
    public int hitCount;
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        user.StartCoroutine(MultiHitCoroutine(user, target));
    }




    private IEnumerator MultiHitCoroutine(Profile user, Profile target)
    {
        for (int i = 0; i < hitCount; i++)
        {
            target.AddToHealth(-user.currentTechnical, user);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
