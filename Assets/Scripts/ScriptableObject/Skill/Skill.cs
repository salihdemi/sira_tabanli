using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class Skill : ScriptableObject
{
    public string _name;

    public TargetType targetType = TargetType.enemy;//a�a�� inecek

    //private AnimationClip clip;
    //private AudioClip clip;


    [Header("Need")]
    public float healthCost;
    public float staminaCost;
    public float manaCost;
    protected void Consume(Profile user)
    {
        user.AddToHealth(-healthCost, user);
        user.AddToMana(-manaCost);
        user.AddToStamina(-staminaCost);
    }//!


    [Header("Skill")]
    public Sprite sprite;
    public AnimationClip effectClip;
    public AudioClip audioClip;
    public abstract IEnumerator Method(Profile user, Profile target);
    public virtual IEnumerator Method(Profile user, Profile target, float damage)
    {
        yield return Method(user, target);
    }

    protected void PlayAudio()
    {
        FightAudioPlayer.Instance?.Play(audioClip);
    }
    protected void PlayAnimation(Profile owner, string animationTrigger)
    {
        owner.SetTrigger(animationTrigger);
    }

    protected void PlayEffect()
    {
        EffectPlayer.Instance?.Play(effectClip);
    }



}
public enum TargetType
{
    enemy,
    ally,

    self,
    allEnemy,
    allAlly,
    all,

    none
}
