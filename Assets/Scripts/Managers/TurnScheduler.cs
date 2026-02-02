using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public static class TurnScheduler
{
    public static List<Profile> aliveProfiles;
    public static List<Profile> orderedProfiles;//hiza gore siralan
    public static int order;

    public static List<AllyProfile> ActiveAllyProfiles = new List<AllyProfile>();
    public static List<EnemyProfile> ActiveEnemyProfiles = new List<EnemyProfile>();

    public static event Action onStartPlay;
    public static event Action onStartTour;

    private static Coroutine playCoroutine;

    private static CorRunner runner;

    //veri tutan
    //hamleler yaparken kullanýlan

    public static void SetAliveProfiles(List<AllyProfile> AllyProfiles, List<EnemyProfile> EnemyProfiles)
    {

        ActiveAllyProfiles = AllyProfiles;
        ActiveEnemyProfiles = EnemyProfiles;

        aliveProfiles = AllyProfiles.Cast<Profile>()
                           .Concat(EnemyProfiles.Cast<Profile>())
                           .ToList();
    }

    public static void SortProfilesWithSpeed()
    {
        order = 0;
        orderedProfiles = aliveProfiles.OrderByDescending(p => p.GetSpeed()).ToList();

    }

    public static void RemoveFromQueue(Profile deadProfile)
    {
        if (aliveProfiles.Contains(deadProfile))
        {
            aliveProfiles.Remove(deadProfile);
        }
    }





    public static void StartTour()
    {
        //Debug.Log("starttour");
        onStartTour.Invoke();
        SortProfilesWithSpeed();
        CheckNextCharacter();
    }
    public static void CheckNextCharacter()
    {
        if (order == aliveProfiles.Count)
        {
            //Debug.Log("tüm hamleler yapýldý");

            //oynat
            PlayF(orderedProfiles);
        }
        else
        {
            LetNextPlayertoPlay();
        }
    }
    public static void LetNextPlayertoPlay()
    {
        order++;
        aliveProfiles[order - 1].LungeStart();
    }
    public static void FinishTour()
    {
        //Debug.Log("finishtour");


        //Debug.Log("fintour");
        StartTour();
    }





    public static void PlayF(List<Profile> orderedProfiles)
    {
        //Debug.Log("Oynat");
        onStartPlay.Invoke();


        if (runner == null)
        {
            // Yeni bir GameObject oluþtur ve scripti ona ekle
            GameObject go = new GameObject("TurnCoroutineRunner");
            runner = go.AddComponent<CorRunner>();
            // Dövüþ bitince silinmemesi gerekiyorsa: Object.DontDestroyOnLoad(go);
        }
        playCoroutine = runner.StartCor(Play(orderedProfiles));
        
    }

    public static IEnumerator Play(List<Profile> profiles)
    {
        yield return null;
        for (int i = 0; i < profiles.Count; i++)
        {
            Profile profile = profiles[i];

            if (playCoroutine != null)
            {
                profile.Play();
            }
            profile.ClearSkillAndTarget();
            yield return new WaitForSeconds(.1f);

        }

        FinishTour();
        playCoroutine = null;
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
                runner.StopCor(playCoroutine);
                playCoroutine = null;
            }

            FightManager.instance.LoseFight();
        }
        else if (ActiveEnemyProfiles.Count == 0)
        {
            if (playCoroutine != null)
            {
                runner.StopCor(playCoroutine);
                playCoroutine = null;
            }

            FightManager.instance.WinFight();
        }
    }
}
public class CorRunner : MonoBehaviour
{// IEnumerator alýr, Coroutine döndürür
    public Coroutine StartCor(IEnumerator corMethod)
    {
        if (corMethod != null)
        {
            return StartCoroutine(corMethod);
        }

        Debug.LogWarning("Baþlatýlmak istenen IEnumerator null!");
        return null;
    }

    // Baþlatýlmýþ bir Coroutine referansýný durdurur
    public void StopCor(Coroutine runningCor)
    {
        if (runningCor != null)
        {
            StopCoroutine(runningCor);
        }
    }
}