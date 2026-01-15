using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TargetingSystem : MonoBehaviour
{
    private void Awake()
    {
        AllyProfile.OnSkillChosen += StartTargeting;
    }
    private void OnDestroy()
    {
        AllyProfile.OnSkillChosen -= StartTargeting;
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

        // Sahnedeki tüm profilleri bul ve sadece uygun olanlarý aktif et
        List<Profile> allProfiles = FightManager.instance.turnScheduler.aliveProfiles;//!
        foreach (Profile p in allProfiles)
        {
            bool isValid = CheckIfValid(p, skill.targetType);
            p.view.SetSelectable(isValid); // Butonu aç/kapat ve görsel efekt ver
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

        // Her þeyi temizle
        //currentCaster.ClearLungeAndTarget();
    }
}