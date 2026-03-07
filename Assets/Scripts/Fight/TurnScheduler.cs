using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public static List<ProfileLungeHandler> profilesThatWillLunge = new List<ProfileLungeHandler>();

    public static void StartTour()
    {
        foreach (Profile profile in FightManager.AllyProfiles) profile.stats.talimsan?.OnTourStart(profile);
        foreach (Profile profile in FightManager.EnemyProfiles) profile.stats.talimsan?.OnTourStart(profile);
        onTourStart?.Invoke();


        StartLunges();
    }
    private static void StartLunges()
    {
        profilesThatWillLunge.Clear();
        foreach (Profile profile in FightManager.EnemyProfiles)
        {
            profilesThatWillLunge.Add(profile.lungeHandler);
        }
        foreach (Profile profile in FightManager.AllyProfiles)
        {
            profilesThatWillLunge.Add(profile.lungeHandler);
        }


        CheckNextProfileToLunge();
    }
    public static void CheckNextProfileToLunge()
    {
        if (order == profilesThatWillLunge.Count)//oynat
        {
            onStartPlay?.Invoke();
            i = 0;
            SortProfilesWithSpeed();
            PlayNextPerson();
        }
        else//devam et
        {
            LetNextProfileToLunge();
        }
    }
    private static void LetNextProfileToLunge()
    {
        ProfileLungeHandler lungeHandler = profilesThatWillLunge[order];
        order++;

        lungeHandler.LungeStart();
    }




    public static void Back()
    {
        if (TargetingSystem.IsTargeting) TargetingSystem.CancelTargeting();
        else
        {
            order -= 2; // ilk dostta geri yaparsan son düþmana döner hamlesini seçtirip geri ilk düþmana gelir. þimdilik çalýþýyorsa dokunma!
            LetNextProfileToLunge();
        }
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

        StartTour();
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
    //yapay zeka
    private static IEnumerator ProcessQueue()
    {
        isBusy = true;
        while (actionQueue.Count > 0)
        {
            skipRequested = false; // Her yeni aksiyon baþýnda sýfýrla
            bool actionFinished = false;

            // 1. Sýradaki aksiyonu al
            IEnumerator nextAction = actionQueue.Dequeue();

            // 2. Aksiyonu takip edebileceðimiz bir sarmalayýcý ile baþlat
            // 'actionFinished' deðiþkenini aksiyon bitince true yapacak þekilde gönderiyoruz
            Coroutine currentCoroutine = FightPanelObjectPool.instance.StartCoroutine(
                RunActionWrapper(nextAction, () => actionFinished = true)
            );

            // 3. Ya aksiyonun bitmesini ya da skip istenmesini bekle
            while (!actionFinished)
            {
                if (skipRequested)
                {
                    // TIKLANDI: Aksiyonu anýnda durdur
                    FightPanelObjectPool.instance.StopCoroutine(currentCoroutine);
                    skipRequested = false;
                    break; // Bu while döngüsünden çýk, sýradaki Dequeue'ya geç
                }
                yield return null; // Bir sonraki frame'e kadar bekle
            }
            
            // Eðer aksiyonlar arasýnda çok az nefes aldýrmak istersen buraya 
            // yield return new WaitForSeconds(0.1f); ekleyebilirsin.
        }

        isBusy = false;
        // TÜM EFEKTLER BÝTTÝ
        PlayNextPerson();
    }

    // Yardýmcý Metot: Aksiyon bittiðinde haber verir
    private static IEnumerator RunActionWrapper(IEnumerator actualAction, Action onComplete)
    {
        yield return FightPanelObjectPool.instance.StartCoroutine(actualAction);
        onComplete?.Invoke();
    }















    public static bool skipRequested = false; // Týklama yapýldýðýnda true olacak
    public static float actionDelay = 1.0f;   // Aksiyon bittikten sonraki ekstra bekleme süresi
    public static void RequestSkip()
    {
        if (isBusy)
        {
            skipRequested = true;
            Debug.Log("Bekleme süresi atlandý!");
        }
    }
}