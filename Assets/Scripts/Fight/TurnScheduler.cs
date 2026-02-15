using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

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





    public static void StartTourLunges()
    {
        Debug.Log("starttourLunges");
        onTourLungesStart.Invoke();
        SortProfilesWithSpeed();
        CheckNextCharacter();
    }
    public static void CheckNextCharacter()
    {
        if (order == aliveProfiles.Count)
        {
            //Debug.Log("tüm hamleler yapýldý");

            //oynat
            Play(orderedProfiles);
        }
        else
        {
            LetNextPlayertoPlay();
        }
    }
    private static void LetNextPlayertoPlay()
    {
        order++;
        aliveProfiles[order - 1].LungeStart();
    }
    private static void FinishTour()
    {
        Debug.Log("finishtour");

        onTourEnd.Invoke();

        StartTourLunges();
    }





    private static void Play(List<Profile> profiles)
    {
        Debug.Log("Oynat");
        onStartPlay.Invoke();

        MakeRunner();

        playCoroutine = runner.StartCor(PlaySomeone(profiles, 0));

    }
    private static IEnumerator PlaySomeone(List<Profile> profiles, int i)
    {
        Profile profile = profiles[i];

        profile.PlayIfAlive();

        yield return new WaitForSeconds(profile.currentUseable.GetTime());

        profile.ClearSkillAndTarget();

        if(i + 1 < profiles.Count) playCoroutine = runner.StartCor(PlaySomeone(profiles, i + 1));
        else
        {
            playCoroutine = null;

            FinishTour();
        }
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

            FightManager.LoseFight();
        }
        else if (ActiveEnemyProfiles.Count == 0)
        {
            if (playCoroutine != null)
            {
                runner.StopCor(playCoroutine);
                playCoroutine = null;
            }

            FightManager.WinFight();
        }
    }









    private static void MakeRunner()
    {
        if (runner == null)
        {
            // Yeni bir GameObject oluþtur ve scripti ona ekle
            GameObject go = new GameObject("TurnCoroutineRunner");
            runner = go.AddComponent<CorRunner>();
            // Dövüþ bitince silinmemesi gerekiyorsa: Object.DontDestroyOnLoad(go);
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