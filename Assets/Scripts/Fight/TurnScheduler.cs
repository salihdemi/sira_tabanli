using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TurnScheduler
{
    private static List<ProfileLungeHandler> aliveProfiles;
    public static List<ProfileLungeHandler> orderedProfiles;//hiza gore siralan
    public static int order;

    public static List<AllyProfileLungeHandler> ActiveAllyProfiles = new List<AllyProfileLungeHandler>();
    public static List<EnemyProfileLungeHandler> ActiveEnemyProfiles = new List<EnemyProfileLungeHandler>();

    public static event Action onStartPlay;
    public static event Action onTourStart;
    public static event Action onTourEnd;

    private static Coroutine playCoroutine;

    #region ProfileHandlers
    public static void SetAliveProfiles(List<Profile> allyProfiles, List<Profile> enemyProfiles)
    {
        ActiveAllyProfiles = allyProfiles
        .Select(p => (AllyProfileLungeHandler)p.lungeHandler)
        .ToList();
        ActiveEnemyProfiles = enemyProfiles
        .Select(p => (EnemyProfileLungeHandler)p.lungeHandler)
        .ToList();

        aliveProfiles = allyProfiles.Cast<ProfileLungeHandler>()
                                .Concat(enemyProfiles.Cast<ProfileLungeHandler>())
                                .ToList();
    }
    public static List<ProfileLungeHandler> GetAliveProfiles()
    {
        return aliveProfiles ?? new List<ProfileLungeHandler>();//Ne yaptýgýný bilmiyorum??!!
        /*
        SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        return aliveProfiles;*/
    }
    public static void SortProfilesWithSpeed()
    {
        order = 0;
        orderedProfiles = aliveProfiles.OrderByDescending(p => p.profile.currentStrength).ToList();

    }
    private static void RemoveFromQueue(Profile deadProfile)
    {
        if (aliveProfiles.Contains(deadProfile.lungeHandler))
        {
            aliveProfiles.Remove(deadProfile.lungeHandler);
        }
    }

    #endregion



    #region LungeSequence
    public static void StartTourLunges()
    {
        foreach (AllyProfileLungeHandler lungeHandler in ActiveAllyProfiles) lungeHandler.profile.stats.talimsan?.OnTourStart(lungeHandler.profile);
        foreach (EnemyProfileLungeHandler lungeHandler in ActiveEnemyProfiles) lungeHandler.profile.stats.talimsan?.OnTourStart(lungeHandler.profile);

        onTourStart.Invoke();
        SortProfilesWithSpeed();



        CheckNextCharacterToLunge();
    }
    public static void CheckNextCharacterToLunge()
    {
        if (order == aliveProfiles.Count)//oynat
        {
            onStartPlay?.Invoke();
            i = 0;
            PlayNextPerson();
        }
        else//devam et
        {
            foreach (AllyProfileLungeHandler profile in ActiveAllyProfiles) profile.profile.stats.talimsan?.OnTourEnd(profile.profile);
            foreach (EnemyProfileLungeHandler profile in ActiveEnemyProfiles) profile.profile.stats.talimsan?.OnTourEnd(profile.profile);
            LetNextPlayertoLunge();
        }
    }
    private static void LetNextPlayertoLunge()
    {
        order++;
        Profile profile = aliveProfiles[order - 1].profile;//-1?

        profile.stats.talimsan?.OnTourStart(profile);
        profile.lungeHandler.LungeStart();
    }

    #endregion

    #region PlaySequence

    private static int i = 0;
    public static void PlayNextPerson()
    {
        if (i >= orderedProfiles.Count)
        {
            i = 0;
            FinishTour();
            return;
        }

        ProfileLungeHandler profile = orderedProfiles[i++];

        bool isPlayed = profile.Play();

        if (!isPlayed)
        {
            PlayNextPerson();
        }
    }

    private static void FinishTour()
    {
        onTourEnd.Invoke();

        StartTourLunges();
    }

    #endregion









    private static Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();
    public static bool isBusy = false;


    // Yeni bir olay (saldýrý veya týlsým) eklendiðinde buraya gelir
    public static void AddAction(IEnumerator action)
    {
        actionQueue.Enqueue(action);
        if (!isBusy) FightPanelObjectPool.instance.StartCoroutine(ProcessQueue());
    }

    private static IEnumerator ProcessQueue()
    {
        isBusy = true;
        while (actionQueue.Count > 0)
        {
            yield return FightPanelObjectPool.instance.StartCoroutine(actionQueue.Dequeue());
        }
        isBusy = false;

        // TÜM EFEKTLER BÝTTÝ, ÞÝMDÝ SIRADAKÝ KÝÞÝYE GEÇEBÝLÝRÝZ
        PlayNextPerson();
    }











    public static void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden çýkar
        if (deadProfile is Profile ally) ActiveAllyProfiles.Remove((AllyProfileLungeHandler)ally.lungeHandler);
        else if (deadProfile is Profile enemy) ActiveEnemyProfiles.Remove((EnemyProfileLungeHandler)enemy.lungeHandler);

        RemoveFromQueue(deadProfile);

        // Savaþ bitti mi kontrol et
        if (ActiveAllyProfiles.Count == 0)
        {
            if (playCoroutine != null)
            {
                //oynat corosunu durdur
                //runner.StopCor(playCoroutine);
                playCoroutine = null;
            }

            FightManager.LoseFight();
        }
        else if (ActiveEnemyProfiles.Count == 0)
        {
            if (playCoroutine != null)
            {
                //runner.StopCor(playCoroutine);
                playCoroutine = null;
            }

            FightManager.WinFight();
        }
    }









}