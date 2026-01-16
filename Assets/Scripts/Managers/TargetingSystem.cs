using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TargetingSystem : MonoBehaviour
{
    public TurnScheduler turnScheduler;
    private void Awake()
    {
        AllyProfile.OnAnyAllyChoseSkill += StartTargeting;
        ProfileView.OnAnyProfileClicked += OnProfileClicked;
    }
    private void OnDestroy()
    {
        AllyProfile.OnAnyAllyChoseSkill -= StartTargeting;
        ProfileView.OnAnyProfileClicked -= OnProfileClicked;
    }






    private _Skill selectedSkill;
    private Profile currentCaster;


    private List<Profile> activeProfileButtons = new List<Profile> { };
    public enum TargetType
    {
        self,
        enemy,
        allEnemy,
        allAlly,
        ally
    }


    public void StartTargeting(Profile caster, _Skill skill)
    {
        selectedSkill = skill;
        currentCaster = caster;

        List<Profile> allProfiles = turnScheduler.orderedProfiles;//!

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

    private bool CheckIfValid(Profile target, TargetType type)
    {
        // Hedef geçerli mi kontrolü (Düþman mý? Dost mu?)
        if (type == TargetType.enemy) return target is EnemyProfile;
        if (type == TargetType.ally) return target is AllyProfile;
        return true;
    }

    public void OnProfileClicked(Profile clickedProfile)
    {
        if (selectedSkill == null) return;

        // Skilli uygula
        currentCaster.SetTarget(clickedProfile);
        CloseButtons();
    }
    private void CloseButtons()
    {
        foreach (Profile profile in activeProfileButtons)
        {
            profile.view.SetSelectable(false);
        }
        activeProfileButtons.Clear();
    }
}