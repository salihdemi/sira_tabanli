using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{

    public override void TurnStart()
    {
        onTurnStarted?.Invoke();

        CharacterActionPanel.instance.WriteThings(this);

        CharacterActionPanel.instance.gameObject.SetActive(true);

    }
    public override void TurnEnd()
    {
        onTurnEnded?.Invoke();
        foreach (Profile profile in FightManager.instance.EnemyProfiles)
        {
            profile.button.interactable = false;
            profile.button.onClick.RemoveAllListeners();
        }

        foreach (Profile profile in FightManager.instance.AllyProfiles)
        {
            profile.button.interactable = false;
            profile.button.onClick.RemoveAllListeners();
        }

        CharacterActionPanel.instance.gameObject.SetActive(false);

        FightManager.instance.CheckNextCharacter();
    }
    public override void SetLunge(_Skill skill)
    {
        //secili saldýrýyý iþaretle
        Lunge = skill.Method;

        CharacterActionPanel.instance.DisableAllPanels();

        //hedef seçecek
        OpenPickTargetMenu(skill);
    }
    public override void OpenPickTargetMenu(_Skill skill)
    {
        Debug.Log(skill.targetType);
        CharacterActionPanel.instance.gameObject.SetActive(false);
        if (skill.targetType == TargetingSystem.TargetType.enemy)
        {
            foreach (Profile profile in FightManager.instance.EnemyProfiles)
            {
                profile.button.interactable = true;
                profile.button.onClick.AddListener(() => SetTarget(profile));
            }
        }
        else if (skill.targetType == TargetingSystem.TargetType.ally)
        {
            foreach (Profile profile in FightManager.instance.AllyProfiles)
            {
                profile.button.interactable = true;
                profile.button.onClick.AddListener(() => SetTarget(profile));
            }
        }
    }
    public void SetTarget(Profile profile)
    {
        Target = profile;

        TurnEnd();
    }
}
