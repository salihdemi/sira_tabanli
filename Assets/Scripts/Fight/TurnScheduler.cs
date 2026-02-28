using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TurnScheduler
{
    public static List<ProfileLungeHandler> orderedProfiles;//hiza gore siralan
    public static int order;


    public static event Action onStartPlay;
    public static event Action onTourStart;
    public static event Action onTourEnd;

    public static Coroutine playCoroutine;

    #region ProfileHandlers
    public static void SortProfilesWithSpeed()
    {
        order = 0;
        orderedProfiles = FightManager.AllyProfiles
        .Concat(FightManager.EnemyProfiles) // Ýki listeyi birleþtir
        .OrderByDescending(p => p.stats.speed) // Profile içindeki hýza göre sýrala
        .Select(p => p.lungeHandler) // Profile'dan Handler'a geçiþ yap
        .ToList(); // Listeye dök

    }
    #endregion



    #region LungeSequence
    public static void StartTourLunges()
    {


        foreach (Profile profile in FightManager.AllyProfiles)  profile.stats.talimsan?.OnTourStart(profile);
        foreach (Profile profile in FightManager.EnemyProfiles) profile.stats.talimsan?.OnTourStart(profile);

        onTourStart?.Invoke();
        SortProfilesWithSpeed();



        CheckNextCharacterToLunge();
    }
    public static void CheckNextCharacterToLunge()
    {
        if (order == orderedProfiles.Count)//oynat
        {
            onStartPlay?.Invoke();
            i = 0;
            PlayNextPerson();
        }
        else//devam et
        {
            LetNextPlayertoLunge();
        }
    }
    private static void LetNextPlayertoLunge()
    {
        Debug.Log(order);
        ProfileLungeHandler lungeHandler = orderedProfiles[order];//Hiz sirasina göre yapmamasý lazým, öylee mi yapýyor
        Debug.Log(lungeHandler.profile.stats._name);
        order++;

        lungeHandler.profile.stats.talimsan?.OnTourStart(lungeHandler.profile);
        lungeHandler.LungeStart();
    }




    public static void Back()
    {
        Debug.Log("back");
        order--;
        LetNextPlayertoLunge();
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
        foreach (Profile profile in FightManager.AllyProfiles) profile.stats.talimsan?.OnTourEnd(profile);//direkt ontourende abone etsek?!
        foreach (Profile profile in FightManager.EnemyProfiles) profile.stats.talimsan?.OnTourEnd(profile);
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




















}