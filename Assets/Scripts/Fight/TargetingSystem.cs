using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public static class TargetingSystem
{
    private static Skill selectedSkill;
    private static Profile currentCaster;


    private static List<ProfileButtonHandler> activeButtonHandlers = new List<ProfileButtonHandler> { };

    public static bool IsTargeting => selectedSkill != null;


    public static void StartTargeting(Profile caster, Skill skill)
    {
        selectedSkill = skill;
        currentCaster = caster;

        List<ProfileButtonHandler> allProfiles = TurnScheduler.profilesThatWillLunge.Select(p => p.buttonHandler).ToList();

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
    public static void CancelTargeting()
    {
        if (currentCaster == null) return;

        // 1. Görsel butonlarý temizle
        CloseButtons();

        // 2. State'i temizle
        selectedSkill = null;

        // 3. Menüyü tekrar aç (Oyuncu geri döndüðü için seçim panelini görmeli)
        if (CharacterActionPanel.instance != null)
        {
            CharacterActionPanel.instance.OpenWriteThings(currentCaster);
        }

        currentCaster = null;
    }
    private static bool CheckIfValid(Profile target, TargetType type)
    {
        // Hedef geçerli mi kontrolü (Düþman mý? Dost mu?)
        if (type == TargetType.enemy) return !target.isAlly;
        else if (type == TargetType.ally) return target.isAlly;
        else return true;
    }

    public static void OnProfileClicked(Profile clickedProfile)
    {
        if (selectedSkill == null) return;

        // Skilli uygula
        currentCaster.lungeHandler.ChooseTarget(clickedProfile);
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