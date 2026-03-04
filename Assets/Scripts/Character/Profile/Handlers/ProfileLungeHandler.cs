using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class ProfileLungeHandler : MonoBehaviour
{
    public Profile profile;
    public ProfileButtonHandler buttonHandler;

    [HideInInspector] protected Skill currentSkill;
    [HideInInspector] protected Profile currentTarget;

    public abstract void LungeStart();
    public abstract void ChooseSkill(Skill skill);
    public abstract void ChooseTarget(Profile profile);
    public void FinishLunge()
    {
       TurnScheduler.CheckNextProfileToLunge();
    }


    public bool Play()
    {
        if (profile.stats.isDied) return false;

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