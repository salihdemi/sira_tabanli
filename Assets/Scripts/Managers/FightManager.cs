

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class FightManager : MonoBehaviour
{

    [SerializeField] private BattleSpawner battleSpawner;
    [SerializeField] private TurnScheduler turnScheduler;
    [SerializeField] private PartyManager  partyManager;

    [SerializeField] private GameObject fightPanel;



    [Header("Profiles")]
    [HideInInspector] public List<AllyProfile>  ActiveAllyProfiles  = new List<AllyProfile>();
    [HideInInspector] public List<EnemyProfile> ActiveEnemyProfiles = new List<EnemyProfile>();


    public static event Action OnFightStart, OnFightEnd;



    private void Awake()
    {
        Profile.OnSomeoneDie += HandleProfileDeath;
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable += StartFight;
        TurnScheduler.PlayTime += PlayF;
    }
    private void OnDestroy()
    {
        Profile.OnSomeoneDie -= HandleProfileDeath;
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable -= StartFight;
    }







    public void StartFight(EnemyData[] enemyDatas)
    {
        #region NullCheck
        if (partyManager.party.Length < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemyDatas.Length < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
        #endregion

        OnFightStart.Invoke();

        fightPanel.SetActive(true);

        AllyData[] allyDatas = partyManager.party;


        ActiveAllyProfiles = battleSpawner.SpawnAllies(allyDatas);
        ActiveEnemyProfiles = battleSpawner.SpawnEnemies(enemyDatas);


        turnScheduler.SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        turnScheduler.SortProfilesWithSpeed();

        //battleSpawner.ResetStats(AllyProfiles);

        turnScheduler.StartTour();
    }
    
    public void WinFight()
    {
        //Ödül ver
        //moveable
        FinishFight();
    }
    public void LoseFight()
    {
        //ölüm ekraný* vs
        //Son save e donme
        FinishFight();
    }
    
    public void FinishFight()
    {
        OnFightEnd.Invoke();

        battleSpawner.ClearBattlefield();

        fightPanel.SetActive(false);
    }




    public void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden çýkar
             if (deadProfile is AllyProfile  ally)  ActiveAllyProfiles. Remove(ally);
        else if (deadProfile is EnemyProfile enemy) ActiveEnemyProfiles.Remove(enemy);

        turnScheduler.RemoveFromQueue(deadProfile);

        // Savaþ bitti mi kontrol et
             if (ActiveAllyProfiles .Count == 0) LoseFight();
        else if (ActiveEnemyProfiles.Count == 0) WinFight();
    }







    public void PlayF(List<Profile> orderedProfiles)
    {
        StartCoroutine(Play(orderedProfiles));
    }

    public IEnumerator Play(List<Profile> profiles)
    {
        Debug.Log("Oynat");
        for (int i = 0; i < profiles.Count; i++)
        {
            Profile profile = profiles[i];

            if (!profile)//isdied kontrolu olmalý
            {
                //caster olmus ona göre yaz
                continue;
            }
            if (!profile.currentTarget)//isdied kontrolu olmalý
            {
                //hedef olmus ona göre yaz-- sovalyenin canavara vurmasini gerektiren bir durum yok
                continue;
            }


            // Sadece bir hamle yapýldýysa bekleme yap
            yield return new WaitForSeconds(1f);

            profile.Play();
            profile.ClearSkillAndTarget();

        }

        turnScheduler.FinishTour();
    }


}
