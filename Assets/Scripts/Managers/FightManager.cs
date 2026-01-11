

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;
    FightManager()
    {
        instance = this;
    }

    public BattleSpawner battleSpawner;
    public TurnScheduler turnScheduler;

    //private List<Profile> Profiles = new List<Profile> { };


    //public Animator animator;
    [Header("Profiles")]
    [HideInInspector] public List<AllyProfile>   AllyProfiles = new List<AllyProfile>();
    [HideInInspector] public List<EnemyProfile> EnemyProfiles = new List<EnemyProfile>();



    private int characterOrder;


    public void StartFight(AllyData[] allyDatas, EnemyData[] enemyDatas)//Fonksiyonla!
    {
        if (MainCharacterMoveable.instance.party.Length < 1)
        {Debug.LogError("Parti boþ");return; }
        if (enemyDatas.Length < 1)
        {Debug.LogError("Düþman partisi boþ");return; }
        //------------------------------------------------------
        //------------------------------------------------------
        //------------------------------------------------------
        gameObject.SetActive(true);

        AllyProfiles = battleSpawner.SpawnAllies(allyDatas);
        EnemyProfiles = battleSpawner.SpawnEnemies(enemyDatas);

        turnScheduler.SetProfiles(AllyProfiles, EnemyProfiles);
        turnScheduler.SortProfilesWithSpeed();

        ResetStats();





        turnScheduler.StartTour();
    }
    public void LoseFight()
    {
        //ölüm ekraný* vs
        //ClearCharacters();
    }
    public void FinishFight()
    {
        //Ödül ver*
        ClearCharacters();
        //moveable
        CharacterActionPanel.instance.gameObject.SetActive(false);
        //gameObject.SetActive(false);
        battleSpawner.ClearBattlefield();
    }







    private void ResetStats()
    {
        for (int i = 0; i < AllyProfiles.Count; i++)
        {
            Profile character = AllyProfiles[i];  
            character.ResetStats();
        }
    }

    private void ClearCharacters()
    {

        for (int i = 0; i < AllyProfiles.Count; i++)
        { Destroy(AllyProfiles[i].gameObject); }
        AllyProfiles.Clear();

            
        for (int i = 0; i < EnemyProfiles.Count; i++)
        { Destroy(EnemyProfiles[i].gameObject); }
        EnemyProfiles.Clear();
    }//!


    public void CheckDieAlly()
    {
        for (int i = 0; i < AllyProfiles.Count; i++)
        {
            Profile profile = AllyProfiles[i];
            if (profile.IsDied())
            {
                KillCharacter(profile);


                if (AllyProfiles.Count == 0)
                {
                    LoseFight();
                }

                else if (EnemyProfiles.Count == 0)
                {
                    FinishFight();
                }
                else
                {
                    turnScheduler.StartTour();
                }
            }


        }
    }
    public void CheckDieEnemy()
    {
        for (int i = 0; i < EnemyProfiles.Count; i++)
        {
            Profile profile = EnemyProfiles[i];
            if (profile.IsDied())
            {
                KillCharacter(profile);


                if (EnemyProfiles.Count == 0)
                {
                    FinishFight();
                }
                else
                {
                    turnScheduler.StartTour();
                }
            }


        }
    }




    private void KillCharacter(Profile profile)
    {

        if (profile is AllyProfile)
        {
            AllyProfile ally = (AllyProfile)profile;
            AllyProfiles.Remove(ally);

            Destroy(ally.gameObject);


        }
        else if (profile is EnemyProfile)
        {
            EnemyProfile enemy = (EnemyProfile)profile;
            EnemyProfiles.Remove(enemy);

            Destroy(enemy.gameObject);

        }
    }


    public IEnumerator Play(List<Profile> profiles)
    {
        Debug.Log("Oynat");
        for (int i = 0;i < profiles.Count; i++)
        {
            Profile profile = profiles[i];
            profile.Lunge(profile, profile.Target);//Hamleyi yap
            profile.ClearLungeAndTarget();//Hamleyi temizle

            yield return new WaitForSeconds(1);
        }

        CheckDieAlly();
        CheckDieEnemy();

    }


}
