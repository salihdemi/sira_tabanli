using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class ProfileLungeHandler : MonoBehaviour
{
    public Profile profile;
    public ProfileButtonHandler buttonHandler;

    [HideInInspector] public Skill currentSkill;
    [HideInInspector] public Profile currentTarget;

    public abstract void LungeStart();
    public abstract void ChooseSkill(Skill skill);
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
        TurnScheduler.CheckNextAllyToLunge();
    }


    public bool Play()
    {
        if (profile.stats.isDied) return false;

        Debug.Log(currentSkill);
        bool needTarget = currentSkill.targetType == TargetType.enemy || currentSkill.targetType == TargetType.ally;
        bool targetValid = !needTarget || (currentTarget != null && !currentTarget.stats.isDied);

        if (targetValid)
        {
            TurnScheduler.AddAction(currentSkill.Method(profile, currentTarget));
            return true; // Baþarýyla sýraya eklendi
        }

        return false; // Oynayamadý
    }



    /*
    public void ClearSkillAndTarget()//gereksiz mi, birden fazla savaþ desteklemek için?
    {
        currentTarget = null;
        currentSkill = null;
    }*/
}