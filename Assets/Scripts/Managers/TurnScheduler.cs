using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class TurnScheduler : MonoBehaviour
{

    public List<Profile> aliveProfiles;
    public List<Profile> orderedProfiles;//hiza gore siralan
    private int order;

    //veri tutan
    //hamleler yaparken kullanýlan


    public void SetAliveProfiles(List<AllyProfile> AllyProfiles, List<EnemyProfile> EnemyProfiles)
    {
        aliveProfiles = AllyProfiles.Cast<Profile>()
                           .Concat(EnemyProfiles.Cast<Profile>())
                           .ToList();
    }

    public void SortProfilesWithSpeed()
    {
        order = 0;
        orderedProfiles = aliveProfiles.OrderByDescending(p => p.GetSpeed()).ToList();

    }

    public void RemoveFromQueue(Profile deadProfile)
    {
        if (aliveProfiles.Contains(deadProfile))
        {
            aliveProfiles.Remove(deadProfile);
        }
    }





    public void StartTour()
    {
        Debug.Log("starttour");
        SortProfilesWithSpeed();
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
            LetNextPlayertoPlay();
        }
    }
    private void LetNextPlayertoPlay()
    {
        order++;
        orderedProfiles[order - 1].LungeStart();
    }
    public void FinishTour()
    {
        Debug.Log("finishtour");



        StartTour();
    }





}