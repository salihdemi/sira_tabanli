using UnityEngine;
using System.Collections;

public class EffectPlayer : MonoBehaviour
{
    public static EffectPlayer Instance;

    private Animation anim;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animation>();
        if (anim == null) anim = gameObject.AddComponent<Animation>();
    }

    public void Play(AnimationClip clip)
    {
        if (clip == null) return;
        StartCoroutine(PlayClip(clip));
    }

    private IEnumerator PlayClip(AnimationClip clip)
    {
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);
        yield return new WaitForSeconds(clip.length);
        anim.RemoveClip(clip);
    }
}
