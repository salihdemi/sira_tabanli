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



        CheckNextCharacterToLunge();
    }
    
    public static void CheckNextCharacterToLunge()
    {
        if (order == aliveProfiles.Count)
        {
            //Debug.Log("tüm hamleler yapýldý");

            //oynat
            MakeRunner();
            onStartPlay?.Invoke();
            runner.StartCor(PlayNextPerson());
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
    private static void FinishTour()
    {
        Debug.Log("finishtour");

        onTourEnd.Invoke();

        StartTourLunges();
    }




    private static int i = 0;
    public static IEnumerator PlayNextPerson()
    {
        Debug.Log("Oynat" + i);
        if (orderedProfiles.Count > i)
        {
            Profile profile = orderedProfiles[i];
            i++;
            yield return runner.StartCor(profile.Play());
        }
        else
        {
            FinishTour();
        }

        


        ///listeyi al
        ///listenin birincisinin hamlesini yaz
        ///araya gir ve yaz
        ///listenin ikincisini yaz

    }

    public static IEnumerator OnSomethingHappen(string str, float time)
    {
        Debug.Log("something");
        ConsolePanel.instance.WriteConsole(str);

        yield return new WaitForSeconds(time);

        //sonraki

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