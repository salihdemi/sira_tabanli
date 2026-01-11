

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

    private List<Profile> Profiles = new List<Profile> { };


    //public Animator animator;
    [Header("Profiles")]
    [HideInInspector] public List<AllyProfile>   AllyProfiles = new List<AllyProfile>();
    [HideInInspector] public List<EnemyProfile> EnemyProfiles = new List<EnemyProfile>();



    private int characterOrder;


    public void StartFight(AllyData[] allies, EnemyData[] enemies)//Fonksiyonla!
    {
        if (MainCharacterMoveable.instance.party.Length < 1)
        {Debug.LogError("Parti boþ");return; }
        if (enemies.Length < 1)
        {Debug.LogError("Düþman partisi boþ");return; }
        //------------------------------------------------------
        //------------------------------------------------------
        //------------------------------------------------------
        gameObject.SetActive(true);

        AllyProfiles = battleSpawner.SpawnAllies(allies);
        EnemyProfiles = battleSpawner.SpawnEnemies(enemies);

        Profiles = GetAllProfiles();

        ResetStats();

        StartTour();
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




    private void StartTour()
    {
        SortWithSpeed();
        characterOrder = 0;
        Debug.Log("starttour");
        CheckNextCharacter();
    }
    private void SortWithSpeed()
    {
        Profiles.Sort((a, b) => b.GetSpeed().CompareTo(a.GetSpeed()));
    }
    public void CheckNextCharacter()
    {
        Debug.Log(Profiles.Count);
        if (characterOrder == Profiles.Count)
        {
            Debug.Log("tüm hamleler yapýldý");
            
            StartCoroutine(Play());//oynat
        }
        else
        {
            characterOrder++;
            LetNextPlayertoPlay();
        }
    }
    private void LetNextPlayertoPlay()
    {
        Debug.Log(Profiles[characterOrder - 1].name + " hamlesini seçiyor");
        Profiles[characterOrder - 1].TurnStart();
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
        Profiles = new List<Profile> { };

        for (int i = 0; i < AllyProfiles.Count; i++)
        { Destroy(AllyProfiles[i].gameObject); }
        AllyProfiles.Clear();

            
        for (int i = 0; i < EnemyProfiles.Count; i++)
        { Destroy(EnemyProfiles[i].gameObject); }
        EnemyProfiles.Clear();
    }


    public void CheckDie()
    {
        for (int i = 0; i < Profiles.Count; i++)
        {
            Profile profile = Profiles[i];
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
                    StartTour();
                }
            }


        }
    }

    private void KillCharacter(Profile profile)
    {

        if (profile is AllyProfile)
        {
            AllyProfile ally = (AllyProfile)profile;
            Profiles.Remove(ally);
            AllyProfiles.Remove(ally);

            Destroy(ally.gameObject);


        }
        else if (profile is EnemyProfile)
        {
            EnemyProfile enemy = (EnemyProfile)profile;
            Profiles.Remove(enemy);
            EnemyProfiles.Remove(enemy);

            Destroy(enemy.gameObject);

        }
    }


    private IEnumerator Play()
    {
        Debug.Log("Oynat");
        for (int i = 0;i < Profiles.Count; i++)
        {
            Profile profile = Profiles[i];
            profile.Lunge(profile, profile.Target);//Hamleyi yap
            profile.ClearLungeAndTarget();//Hamleyi temizle

            yield return new WaitForSeconds(1);
        }

        CheckDie();

    }


    public List<Profile> GetAllProfiles()
    {
        return AllyProfiles.Cast<Profile>()
                           .Concat(EnemyProfiles.Cast<Profile>())
                           .ToList();
    }
}
