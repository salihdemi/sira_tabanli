using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public static class TargetingSystem
{







    private static Skill selectedSkill;
    private static Profile currentCaster;


    private static List<Profile> activeProfileButtons = new List<Profile> { };


    public static void StartTargeting(Profile caster, Skill skill)
    {
        selectedSkill = skill;
        currentCaster = caster;

        List<ProfileLungeHandler> allProfiles = TurnScheduler.orderedProfiles;//!

        foreach (ProfileLungeHandler p in allProfiles)
        {
            bool isValid = CheckIfValid(p.profile, skill.targetType);
            if (isValid)
            {
                activeProfileButtons.Add(p.profile);
                p.profile.view.SetSelectable(true);
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
        foreach (Profile profile in activeProfileButtons)
        {
            profile.view.SetSelectable(false);
        }
        activeProfileButtons.Clear();
    }
}