using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class TurnScheduler : MonoBehaviour
{

    public List<Profile> profiles;
    public List<Profile> orderedProfiles;//hiza gore siralan
    private int order;

    //veri tutan
    //hamleler yaparken kullanýlan


    public void SetProfiles(List<AllyProfile> AllyProfiles, List<EnemyProfile> EnemyProfiles)
    {
        profiles = AllyProfiles.Cast<Profile>()
                           .Concat(EnemyProfiles.Cast<Profile>())
                           .ToList();
    }

    public void SortProfilesWithSpeed()
    {
        order = 0;
        orderedProfiles = profiles.OrderByDescending(p => p.GetSpeed()).ToList();

    }







    public void StartTour()
    {
        Debug.Log("starttour");
        CheckNextCharacter();
    }
    public void CheckNextCharacter()
    {
        if (order == orderedProfiles.Count)
        {
            Debug.Log("tüm hamleler yapýldý");

            StartCoroutine(FightManager.instance.Play(orderedProfiles));//oynat
        }
        else
        {
            order++;
            LetNextPlayertoPlay();
        }
    }
    private void LetNextPlayertoPlay()
    {
        Debug.Log(profiles[0].name + " hamlesini seçiyor");
        orderedProfiles[0].TurnStart();
    }












    /*

    // Savaþacak olanlarýn sýrasý burada tutulur
    private Queue<Profile> turnQueue = new Queue<Profile>();

    // 1. Kuyruðu Oluþtur ve Sýrala
    public void PrepareQueue(List<Profile> allParticipants)
    {
        turnQueue.Clear();

        // Hýz (Speed) deðerine göre büyükten küçüðe sýrala
        var sortedList = allParticipants
            .OrderByDescending(p => p.GetSpeed())
            .ToList();

        // Sýralanmýþ listeyi kuyruða ekle
        foreach (var p in sortedList)
        {
            turnQueue.Enqueue(p);
        }
    }

    // 2. Sýradaki Karakteri Ver
    public Profile GetNextCharacter()
    {
        if (turnQueue.Count > 0)
        {
            return turnQueue.Dequeue(); // Kuyruðun baþýndaki kiþiyi alýr ve listeden çýkarýr
        }
        return null; // Kuyruk bittiyse (Tur tamamlandýysa) null döner
    }

    // 3. Kuyruk boþ mu kontrolü
    public bool IsQueueEmpty() => turnQueue.Count == 0;

    */
}