using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public static class TargetingSystem
{







    private static Useable selectedSkill;
    private static Profile currentCaster;


    private static List<Profile> activeProfileButtons = new List<Profile> { };
    public enum TargetType
    {
        self,
        enemy,
        allEnemy,
        allAlly,
        ally
    }


    public static void StartTargeting(Profile caster, Useable skill)
    {
        selectedSkill = skill;
        currentCaster = caster;

        List<Profile> allProfiles = TurnScheduler.orderedProfiles;//!

        foreach (Profile p in allProfiles)
        {
            bool isValid = CheckIfValid(p, skill.targetType);
            if (isValid)
            {
                activeProfileButtons.Add(p);
                p.view.SetSelectable(true);
            }
        }

    }

    private static bool CheckIfValid(Profile target, TargetType type)
    {
        // Hedef geçerli mi kontrolü (Düþman mý? Dost mu?)
        if (type == TargetType.enemy) return target is EnemyProfile;
        if (type == TargetType.ally) return target is AllyProfile;
        return true;
    }

    public static void OnProfileClicked(Profile clickedProfile)
    {
        if (selectedSkill == null) return;

        // Skilli uygula
        currentCaster.SetTarget(clickedProfile);
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