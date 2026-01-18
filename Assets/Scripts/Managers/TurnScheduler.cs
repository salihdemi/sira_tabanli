using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class TurnScheduler : MonoBehaviour
{
    public FightManager fightManager;
    /*[HideInInspector]*/ public List<Profile> aliveProfiles;
    /*[HideInInspector]*/ public List<Profile> orderedProfiles;//hiza gore siralan
    private int order;

    /*[HideInInspector]*/ public List<AllyProfile> ActiveAllyProfiles = new List<AllyProfile>();
    /*[HideInInspector]*/ public List<EnemyProfile> ActiveEnemyProfiles = new List<EnemyProfile>();

    public static event Action onStartPlay;
    public static event Action onStartTour;

    private Coroutine playCoroutine;

    //veri tutan
    //hamleler yaparken kullanýlan
    private void Awake()
    {
        Profile.OnSomeoneLungeEnd += CheckNextCharacter;
        Profile.OnSomeoneDie += HandleProfileDeath;
    }
    private void OnDestroy()
    {
        Profile.OnSomeoneLungeEnd -= CheckNextCharacter;
        Profile.OnSomeoneDie -= HandleProfileDeath;
    }

    public void SetAliveProfiles(List<AllyProfile> AllyProfiles, List<EnemyProfile> EnemyProfiles)
    {

        ActiveAllyProfiles = AllyProfiles;
        ActiveEnemyProfiles = EnemyProfiles;

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
        //Debug.Log("starttour");
        onStartTour.Invoke();
        SortProfilesWithSpeed();
        CheckNextCharacter();
    }
    public void CheckNextCharacter()
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
    public void LetNextPlayertoPlay()
    {
        order++;
        aliveProfiles[order - 1].LungeStart();
    }
    public void FinishTour()
    {
        //Debug.Log("finishtour");


        //Debug.Log("fintour");
        StartTour();
    }





    public void PlayF(List<Profile> orderedProfiles)
    {
        //Debug.Log("Oynat");
        onStartPlay.Invoke();
        if (playCoroutine == null)
        {
            playCoroutine = StartCoroutine(Play(orderedProfiles));
        }
    }

    public IEnumerator Play(List<Profile> profiles)
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



    public void HandleProfileDeath(Profile deadProfile)
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
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }

            fightManager.LoseFight();
        }
        else if (ActiveEnemyProfiles.Count == 0)
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }

            fightManager.WinFight();
        }
    }
}