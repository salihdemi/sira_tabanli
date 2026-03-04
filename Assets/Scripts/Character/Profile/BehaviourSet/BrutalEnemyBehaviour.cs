using UnityEngine;

[CreateAssetMenu(fileName = "BrutalBehaviour", menuName = "AI/Behaviours/Brutal")]
public class BrutalEnemyBehaviour : EnemyBehaviourSet
{
    //----------------------------------
    //Canýn az ise can bas
    //art arda 2 kere can basma
    //caný en az olana saldýr
    //----------------------------------
    //----------------------------------
    //attack tekli saldýrý
    //skill 0 heal
    //skill 1 kamikkaze
    //----------------------------------


    public override void DecideLunge(ProfileLungeHandler lungeHandler)
    {
        Profile prof = lungeHandler.profile;

        Skill skill;
        Profile target;

        bool needHeal = !IsHealthEnough(prof, 30);
        // caný az ise
        if (needHeal)
        {
            bool canHeal = prof.IsEnoughForSkill(prof.stats.currentSkills[0]);
            //can basabiliyor ise
            if (canHeal)
            {
                // can bas
                skill = prof.stats.currentSkills[0];
                target = prof;
            }
            //caný az ama iyileþemiyor
            else
            {
                // kamiikaze saldýrýsý
                skill = prof.stats.currentSkills[1];
                target = null;
            }
        }
        //caný var ise
        else
        {
            // saldýr
            skill = prof.stats.attack;
            target = GetLowestHealthAlly();
        }

        ChooseSkill(lungeHandler, skill);
        ChooseTarget(lungeHandler, target);
    }
}
