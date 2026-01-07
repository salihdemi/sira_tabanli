using UnityEngine;
using UnityEngine.UI;

public class EnemyProfile : Profile
{
    public override void Play()
    {
        _Skill currentskill = character.skills[Random.Range(0, character.skills.Count - 1)]; //Random hamle ver
        SetLunge(currentskill);
    }

    public override void Over()
    {
        FightManager.instance.CheckNextCharacter();
    }
    public override void SetLunge(_Skill skill)
    {
        Lunge = skill.Method;//secili saldýrýyý iþaretle

        OpenPickTargetMenu(skill);//Hedef seçme ekranýný aç
    }
    public override void OpenPickTargetMenu(_Skill skill)
    {
        Target = MainCharacterMoveable.instance.party[0].profile;//default hedef!

        Over();
    }

}
