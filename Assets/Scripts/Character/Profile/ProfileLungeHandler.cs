using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ProfileLungeHandler : MonoBehaviour
{
    public Profile profile;

    [HideInInspector] public Skill currentSkill;
    [HideInInspector] public Profile currentTarget;

    public virtual void LungeStart()
    {
        //Debug.Log(name + " hamlesini seçiyor");

        Skill currentskill = profile.stats.attack; //default hamle
        ChooseSkill(currentskill);
    }
    public virtual void ChooseSkill(Skill skill)
    {
        currentSkill = skill;
        if (FightManager.tauntedAlly)
        {
            SetTarget(FightManager.tauntedAlly);
        }
        else
        {
            SetTarget(FightManager.defaultTargetForEnemies);//!default hedef!
        }


    }
    public virtual void SetTarget(Profile profile)
    {
        if (profile == null)//Cok hedefli skillerde
        {
            FinishLunge();
            return;
        }
        currentTarget = profile;
        //lastTargetName = currentTarget.name;

        FinishLunge();//!
    }
    public void FinishLunge()
    {
        TurnScheduler.CheckNextCharacterToLunge();
    }


    public bool Play()
    {
        if (profile.isDied) return false;

        bool needTarget = currentSkill.targetType == TargetType.enemy || currentSkill.targetType == TargetType.ally;
        bool targetValid = !needTarget || (currentTarget != null && !currentTarget.isDied);

        if (targetValid)
        {
            TurnScheduler.AddAction(currentSkill.Method(profile, currentTarget));
            return true; // Baþarýyla sýraya eklendi
        }

        return false; // Oynayamadý
    }




    public void ClearSkillAndTarget()//gereksiz mi, birden fazla savaþ desteklemek için?
    {
        currentTarget = null;
        currentSkill = null;
    }
}