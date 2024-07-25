using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WaveAttributes 
{
    public WaveAttributes(GameObject waveEnemy, int enemyAmount, float spawnDelay = 0)
    {
        this.waveEnemy = waveEnemy;
        this.enemyAmount = enemyAmount;
        this.spawnDelay = spawnDelay;
    }

    public GameObject waveEnemy;
    public int enemyAmount;
    public float spawnDelay;
    //...
    //Buffs or Special Abilities
   
}
