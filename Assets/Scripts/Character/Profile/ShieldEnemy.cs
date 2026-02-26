using UnityEngine;

public class ShieldEnemy : Profile
{
    public override void AddToHealth(float amount, Profile dealer)
    {
        //kalkaný varsa
        if (stats.currentShields.Count > 0 && amount < 0)
        {
            DamageShield(-amount);
        }
        else
        {
            base.AddToHealth(amount, dealer);
        }

    }
    public void AddShield(float amount)
    {
        stats.currentShields.Add(amount);

        NotifyHealthChanged(); // ondamageShield???
        return;
    }
    private void DamageShield(float damage)
    {
        stats.currentShields[0] -= damage;

        if (stats.currentShields[0] <= 0)
        {
            stats.currentShields.RemoveAt(0); // En üstteki kalkaný yok et
            //Debug.Log($"{name} kalkaný kýrýldý! Kalan hasar emildi.");

        }
        else
        {
            //Debug.Log($"{name} kalkaný darbe aldý. Kalan kalkan caný: {stats.currentShields[0]}");
        }

        NotifyHealthChanged(); // ondamageShield???
        return;
    }
}
