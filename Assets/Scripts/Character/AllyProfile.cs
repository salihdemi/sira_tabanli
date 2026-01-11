using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyProfile : Profile
{

    public override void TurnStart()
    {
        base.TurnStart();

        CharacterActionPanel.instance.WriteThings(this);

        CharacterActionPanel.instance.gameObject.SetActive(true);

    }
    public override void TurnEnd()
    {
        base.TurnEnd();
        

        //burayý al
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
    }
    public override void SetLunge(_Skill skill)
    {
        //secili saldýrýyý iþaretle
        Lunge = skill.Method;

        CharacterActionPanel.instance.DisableAllPanels();

        //hedef seçecek
        TargetingSystem.instance.StartTargeting(this, skill);
    }
}
