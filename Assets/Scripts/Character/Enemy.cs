using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Characters/Enemy")]
public class Enemy : CharacterBase
{

    public override void MakeProfile()
    {
        profile = FightManager.instance.MakeEnemyProfile();
        profile.characterData = this;
        profile.gameObject.name = name;
    }
}
