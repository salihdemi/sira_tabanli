using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{

    public override void Play()
    {
        CharacterActionPanel.instance.WriteThings(this);

        CharacterActionPanel.instance.gameObject.SetActive(true);
    }
    public override void Over()
    {
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
        CharacterActionPanel.instance.gameObject.SetActive(false);
        if (skill.isToEnemy)
        {
            foreach (Profile profile in FightManager.instance.EnemyProfiles)
            {
                profile.button.interactable = true;
                profile.button.onClick.AddListener(() => SetTarget(profile));
            }
        }
        else
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

        Over();
    }
}
