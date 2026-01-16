using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class TurnScheduler : MonoBehaviour
{
    public event Action onAllAlliesDie;
    public event Action onAllEnemiesDie;
    [HideInInspector] public List<Profile> aliveProfiles;
    public List<Profile> orderedProfiles;//hiza gore siralan
    public int order;

    [Header("Profiles")]
    [HideInInspector] public List<AllyProfile> ActiveAllyProfiles = new List<AllyProfile>();
    [HideInInspector] public List<EnemyProfile> ActiveEnemyProfiles = new List<EnemyProfile>();
    //veri tutan
    //hamleler yaparken kullanýlan
    private void Awake()
    {
        Profile.OnSomeoneDie += HandleProfileDeath;
        Profile.OnSomeoneLungeEnd += CheckNextCharacter;
    }
    private void OnDestroy()
    {
        Profile.OnSomeoneDie -= HandleProfileDeath;
        Profile.OnSomeoneLungeEnd -= CheckNextCharacter;
    }

    public void SetAliveProfiles()
    {
        aliveProfiles = ActiveAllyProfiles.Cast<Profile>()
                           .Concat(ActiveEnemyProfiles.Cast<Profile>())
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
        Debug.Log(order);
        SortProfilesWithSpeed();
        CheckNextCharacter();
    }
    public void CheckNextCharacter()
    {
        Debug.Log(order+" "+orderedProfiles.Count);
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


    public void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden çýkar
        if (deadProfile is AllyProfile ally) ActiveAllyProfiles.Remove(ally);
        else if (deadProfile is EnemyProfile enemy) ActiveEnemyProfiles.Remove(enemy);

        RemoveFromQueue(deadProfile);

        // Savaþ bitti mi kontrol et
             if (ActiveAllyProfiles .Count == 0) onAllAlliesDie.Invoke();
        else if (ActiveEnemyProfiles.Count == 0) onAllEnemiesDie.Invoke();
    }
}