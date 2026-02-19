using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatManager
{
    //public static CombatManager instance;
    private static Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();
    public static bool isBusy = false;

    //void Awake() => instance = this;

    // Yeni bir olay (saldýrý veya týlsým) eklendiðinde buraya gelir
    public static void AddAction(IEnumerator action)
    {
        actionQueue.Enqueue(action);
        if (!isBusy) FightPanelObjectPool.instance.StartCoroutine(ProcessQueue());
    }

    private static IEnumerator ProcessQueue()
    {
        isBusy = true;
        while (actionQueue.Count > 0)
        {
            // Sýradaki olayý (saldýrý veya týlsým) çalýþtýr ve BÝTMESÝNÝ BEKLE
            yield return FightPanelObjectPool.instance.StartCoroutine(actionQueue.Dequeue());
        }
        isBusy = false;

        // Oda boþaldýysa, TurnScheduler'a "Sýradaki karakteri oynatabilirsin" de
        TurnScheduler.PlayNextPerson();
    }
}