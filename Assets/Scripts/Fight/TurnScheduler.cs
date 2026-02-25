using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TurnScheduler
{
    private static List<Profile> aliveProfiles;
    public static List<Profile> orderedProfiles;//hiza gore siralan
    public static int order;

    public static List<AllyProfile> ActiveAllyProfiles = new List<AllyProfile>();
    public static List<EnemyProfile> ActiveEnemyProfiles = new List<EnemyProfile>();

    public static event Action onStartPlay;
    public static event Action onTourStart;
    public static event Action onTourEnd;

    private static Coroutine playCoroutine;

    #region ProfileHandlers
    public static void SetAliveProfiles(List<AllyProfile> AllyProfiles, List<EnemyProfile> EnemyProfiles)
    {

        ActiveAllyProfiles = AllyProfiles;
        ActiveEnemyProfiles = EnemyProfiles;

        aliveProfiles = AllyProfiles.Cast<Profile>()
                           .Concat(EnemyProfiles.Cast<Profile>())
                           .ToList();
    }
    public static List<Profile> GetAliveProfiles()
    {
        SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        return aliveProfiles;
    }
    public static void SortProfilesWithSpeed()
    {
        order = 0;
        orderedProfiles = aliveProfiles.OrderByDescending(p => p.currentStrength).ToList();

    }
    private static void RemoveFromQueue(Profile deadProfile)
    {
        if (aliveProfiles.Contains(deadProfile))
        {
            aliveProfiles.Remove(deadProfile);
        }
    }

    #endregion



    #region LungeSequence
    public static void StartTourLunges()
    {
        foreach (Profile profile in ActiveAllyProfiles) profile.stats.talimsan?.OnTourStart(profile);
        foreach (Profile profile in ActiveEnemyProfiles) profile.stats.talimsan?.OnTourStart(profile);

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
            foreach (Profile profile in ActiveAllyProfiles) profile.stats.talimsan?.OnTourEnd(profile);
            foreach (Profile profile in ActiveEnemyProfiles) profile.stats.talimsan?.OnTourEnd(profile);
            LetNextPlayertoLunge();
        }
    }
    private static void LetNextPlayertoLunge()
    {
        order++;
        Profile profile = aliveProfiles[order - 1];//-1?

        profile.stats.talimsan?.OnTourStart(profile);
        profile.LungeStart();
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

        Profile profile = orderedProfiles[i++];

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
        if (deadProfile is AllyProfile ally) ActiveAllyProfiles.Remove(ally);
        else if (deadProfile is EnemyProfile enemy) ActiveEnemyProfiles.Remove(enemy);

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