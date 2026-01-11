using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TargetingSystem : MonoBehaviour
{
    public static TargetingSystem instance;
    private void Awake()
    {
        instance = this;
    }
    private _Skill selectedSkill;
    private Profile currentCaster;
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

        CharacterActionPanel.instance.gameObject.SetActive(false);
        // Sahnedeki tüm profilleri bul ve sadece uygun olanlarý aktif et
        List<Profile> allProfiles = FightManager.instance.turnScheduler.profiles;//!
        foreach (Profile p in allProfiles)
        {
            bool isValid = CheckIfValid(p, skill.targetType);
            p.SetSelectable(isValid,currentCaster); // Butonu aç/kapat ve görsel efekt ver
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
        currentCaster.Lunge(currentCaster, currentCaster.Target);

        // Her þeyi temizle
        currentCaster.ClearLungeAndTarget();
    }
}