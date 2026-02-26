using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public static class TargetingSystem
{
    private static Skill selectedSkill;
    private static Profile currentCaster;


    private static List<ProfileButtonHandler> activeButtonHandlers = new List<ProfileButtonHandler> { };


    public static void StartTargeting(Profile caster, Skill skill)
    {
        selectedSkill = skill;
        currentCaster = caster;

        List<ProfileButtonHandler> allProfiles = TurnScheduler.orderedProfiles.Select(p => p.buttonHandler).ToList();//555555555555555!!!

        foreach (ProfileButtonHandler buttonHandler in allProfiles)
        {
            bool isValid = CheckIfValid(buttonHandler.profile, skill.targetType);
            if (isValid)
            {
                activeButtonHandlers.Add(buttonHandler);
                buttonHandler.SetSelectable(true);
            }
        }

    }

    private static bool CheckIfValid(Profile target, TargetType type)
    {
        // Hedef geçerli mi kontrolü (Düþman mý? Dost mu?)
        if (type == TargetType.enemy) return target is Profile;
        if (type == TargetType.ally) return target is Profile;
        return true;
    }

    public static void OnProfileClicked(Profile clickedProfile)
    {
        if (selectedSkill == null) return;

        // Skilli uygula
        currentCaster.lungeHandler.SetTarget(clickedProfile);
        CloseButtons();
    }
    private static void CloseButtons()
    {
        foreach (ProfileButtonHandler buttonHandler in activeButtonHandlers)
        {
            buttonHandler.SetSelectable(false);
        }
        activeButtonHandlers.Clear();
    }
}