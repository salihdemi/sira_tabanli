using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class TurnScheduler : MonoBehaviour
{

    [HideInInspector] public List<Profile> aliveProfiles;
    public static List<Profile> orderedProfiles;//hiza gore siralan
    public static int order;

    //veri tutan
    //hamleler yaparken kullanýlan
    private void Awake()
    {
        Profile.OnSomeoneLungeEnd += CheckNextCharacter;
    }
    private void OnDestroy()
    {
        Profile.OnSomeoneLungeEnd -= CheckNextCharacter;
        
    }

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

            //oynat
            PlayF(orderedProfiles);
        }
        else
        {
            LetNextPlayertoPlay();
        }
    }
    public void LetNextPlayertoPlay()
    {
        order++;
        orderedProfiles[order - 1].LungeStart();
    }
    public void FinishTour()
    {
        Debug.Log("finishtour");



        StartTour();
    }





    public void PlayF(List<Profile> orderedProfiles)
    {
        StartCoroutine(Play(orderedProfiles));
    }

    public IEnumerator Play(List<Profile> profiles)
    {
        Debug.Log("Oynat");
        for (int i = 0; i < profiles.Count; i++)
        {
            Profile profile = profiles[i];


            yield return new WaitForSeconds(1f);

            profile.Play();
            profile.ClearSkillAndTarget();

        }
        yield return new WaitForSeconds(1f);

        FinishTour();
    }
}