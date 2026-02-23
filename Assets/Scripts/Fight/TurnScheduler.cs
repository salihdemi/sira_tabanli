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
    public static event Action onTourLungesStart;
    public static event Action onTourEnd;

    private static Coroutine playCoroutine;


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





    #region LungeSequence
    public static void StartTourLunges()
    {
        Debug.Log("starttourLunges");
        onTourLungesStart.Invoke();
        SortProfilesWithSpeed();



        CheckNextCharacterToLunge();
    }
    public static void CheckNextCharacterToLunge()
    {
        if (order == aliveProfiles.Count)
        {
            //Debug.Log("tüm hamleler yapýldý");

            //oynat
            onStartPlay?.Invoke();
            i = 0;
            PlayNextPerson();
        }
        else
        {
            LetNextPlayertoLunge();
        }
    }
    private static void LetNextPlayertoLunge()
    {
        order++;
        aliveProfiles[order - 1].LungeStart();
    }

    #endregion

    #region PlaySequence

    private static int i = 0;
    public static void PlayNextPerson()
    {

        if (orderedProfiles.Count > i)
        {
            Debug.Log("Oynat" + orderedProfiles[i].name);
            Profile profile = orderedProfiles[i];
            i++;
            if (!profile.isDied)
            {
                profile.Play();
            }
            else
            {
                PlayNextPerson();
            }
        }
        else
        {
            Debug.Log("FinishTour");
            FinishTour();
        }




        ///listeyi al
        ///listenin birincisinin hamlesini yaz
        ///araya gir ve yaz
        ///listenin ikincisini yaz

    }
    /*
    public static void Something(float time, Action onComplete)
    {
        FightPanelObjectPool.instance.StartCoroutine(SomethingHappen(time, onComplete));
    }
    private static IEnumerator SomethingHappen(float time, Action onComplete)
    {

        Debug.Log("SomethingHappen");
        if (onComplete != null) onComplete.Invoke();

        yield return new WaitForSeconds(time);


        PlayNextPerson();
    }
    */
    private static void FinishTour()
    {
        onTourEnd.Invoke();

        StartTourLunges();
    }

    #endregion









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