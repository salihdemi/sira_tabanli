

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;
    FightManager()
    {
        instance = this;
    }

    public BattleSpawner battleSpawner;
    public TurnScheduler turnScheduler;

    [SerializeField] private GameObject fightPanel;



    [Header("Profiles")]
    [HideInInspector] public List<AllyProfile>   AllyProfiles = new List<AllyProfile>();
    [HideInInspector] public List<EnemyProfile> EnemyProfiles = new List<EnemyProfile>();




    public void StartFight(AllyData[] allyDatas, EnemyData[] enemyDatas)
    {
        #region NullCheck
        if (PartyManager.instance.party.Length < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemyDatas.Length < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
        #endregion

        fightPanel.SetActive(true);

        AllyProfiles = battleSpawner.SpawnAllies(allyDatas);
        EnemyProfiles = battleSpawner.SpawnEnemies(enemyDatas);


        turnScheduler.SetAliveProfiles(AllyProfiles, EnemyProfiles);
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
        battleSpawner.ClearBattlefield();

        fightPanel.SetActive(false);
    }




    public void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden çýkar
             if (deadProfile is AllyProfile  ally)   AllyProfiles.Remove(ally);
        else if (deadProfile is EnemyProfile enemy) EnemyProfiles.Remove(enemy);

        turnScheduler.RemoveFromQueue(deadProfile);

        // Savaþ bitti mi kontrol et
             if (AllyProfiles .Count == 0) LoseFight();
        else if (EnemyProfiles.Count == 0) WinFight();
    }









    public IEnumerator Play(List<Profile> profiles)
    {
        Debug.Log("Oynat");
        for (int i = 0;i < profiles.Count; i++)
        {
            Profile profile = profiles[i];
            profile.Play();
            profile.ClearSkillAndTarget();//Hamleyi temizle

            yield return new WaitForSeconds(1);
        }

        turnScheduler.FinishTour();
    }


}
