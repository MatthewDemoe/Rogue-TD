using UnityEngine;

public struct WaveEnemyAttributes 
{
    public WaveEnemyAttributes(GameObject waveEnemy, int enemyAmount, float spawnDelay = 0)
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
