

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

    [SerializeField] private GameObject fightParent;



    [Header("Profiles")]
    [HideInInspector] public List<AllyProfile>   AllyProfiles = new List<AllyProfile>();
    [HideInInspector] public List<EnemyProfile> EnemyProfiles = new List<EnemyProfile>();




    public void StartFight(AllyData[] allyDatas, EnemyData[] enemyDatas)
    {
        #region NullCheck
        if (MainCharacterMoveable.instance.party.Length < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemyDatas.Length < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
        #endregion

        fightParent.SetActive(true);

        AllyProfiles = battleSpawner.SpawnAllies(allyDatas);
        EnemyProfiles = battleSpawner.SpawnEnemies(enemyDatas);

        // --- YENÝ EKLENEN ABONELÝK KISMI ---
        // Oluþturulan tüm karakterleri gez ve olaylarýný baðla
        foreach (var p in AllyProfiles) SubscribeProfileEvents(p);
        foreach (var p in EnemyProfiles) SubscribeProfileEvents(p);
        // ----------------------------------

        turnScheduler.SetAliveProfiles(AllyProfiles, EnemyProfiles);
        turnScheduler.SortProfilesWithSpeed();

        battleSpawner.ResetStats(AllyProfiles);

        turnScheduler.StartTour();
    }

    // Kod kalabalýðý olmamasý için abonelikleri ayrý bir küçük fonksiyona aldýk
    private void SubscribeProfileEvents(Profile profile)
    {
        // Ölüm haberi: FightManager ve TurnScheduler dinliyor
        profile.onProfileDie += HandleProfileDeath;
        profile.onProfileDie += turnScheduler.RemoveFromQueue;

        // Sýra seçimi bitti haberi: Scheduler bir sonrakine geçmek için dinliyor
        profile.onTurnEnded += turnScheduler.CheckNextCharacter;
    }
    public void LoseFight()
    {
        //ölüm ekraný* vs
        //ClearCharacters();
    }
    public void FinishFight()
    {
        //Ödül ver
        //moveable
        CharacterActionPanel.instance.gameObject.SetActive(false);
        battleSpawner.ClearBattlefield();


        fightParent.SetActive(false);
    }





    public void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden çýkar
        if (deadProfile is AllyProfile ally)
            AllyProfiles.Remove(ally);
        else if (deadProfile is EnemyProfile enemy)
            EnemyProfiles.Remove(enemy);

        // Savaþ bitti mi kontrol et
        if (AllyProfiles.Count == 0) LoseFight();
        else if (EnemyProfiles.Count == 0) FinishFight();
    }









    public IEnumerator Play(List<Profile> profiles)
    {
        Debug.Log("Oynat");
        for (int i = 0;i < profiles.Count; i++)
        {
            Profile profile = profiles[i];
            profile.Play();
            profile.ClearLungeAndTarget();//Hamleyi temizle

            yield return new WaitForSeconds(1);
        }


    }


}
