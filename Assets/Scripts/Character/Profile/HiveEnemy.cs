using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveEnemy : Profile
{
    public static List<HiveEnemy> hiveEnemies = new List<HiveEnemy>();

    // Sonsuz döngüyü engellemek için kontrol flag'i
    private static bool _isBalancing = false;

    public override void Setup(PersistanceStats persistanceStats)
    {
        base.Setup(persistanceStats);
        if (!hiveEnemies.Contains(this)) hiveEnemies.Add(this);
    }

    public override void UnSetup()
    {
        base.UnSetup();
        hiveEnemies.Remove(this);
    }

    public override void AddToHealth(float amount, Profile dealer)
    {
        base.AddToHealth(amount, dealer);

        if (!_isBalancing) BalanceHiveHealth();
    }

    private void BalanceHiveHealth()
    {
        if (hiveEnemies.Count <= 1) return;

        _isBalancing = true; // "Þu an canlarý eþitliyorum, AddToHealth çaðrýlýrsa görmezden gel"

        float totalCurrentHealth = 0;

        // 1. Adým: Kolektif toplamlarý hesapla
        foreach (HiveEnemy enemy in hiveEnemies)
        {
            totalCurrentHealth += enemy.stats.currentHealth;
        }

        // 2. Adým: Kolektif doluluk oranýný bul (0 ile 1 arasý)
        float globalHealthRatio = totalCurrentHealth / hiveEnemies.Count;

        // 3. Adým: Herkese payýný geri ver
        foreach (HiveEnemy enemy in hiveEnemies)
        {
            globalHealthRatio = Mathf.Floor(globalHealthRatio);
            // Profile içindeki SetHealth eventleri tetikler ve UI'ý günceller
            enemy.SetHealth(globalHealthRatio);
        }

        TurnScheduler.AddAction(BalanceMessage());//mesaj


        _isBalancing = false; // Ýþlem bitti, kilidi aç
    }

    private IEnumerator BalanceMessage()
    {
        string text = $"{stats._name} hasar aldý, tüm kovancýlar etkilendi";
        ConsolePanel.instance.WriteConsole(text);
        yield return new WaitForSeconds(1f);
    }
}
