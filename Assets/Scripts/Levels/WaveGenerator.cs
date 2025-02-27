using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveGenerator 
{
    const int BASE_DIFFICULTY = 10;
    const int MIN_ENEMY_TYPES = 1;

    const int MAX_ENEMY_TYPES = 3;

    int numWaves = 0;

    public List<Wave> waves { get; private set; } = new();

    public WaveGenerator(int numWaves)
    {
        this.numWaves = numWaves;

        for (int i = 0; i < numWaves; i++)
        {
            waves.Add(GenerateWave(i));
        }
    }

    public Wave GenerateWave(int waveNumber)
    {
        int waveDifficulty = BASE_DIFFICULTY + (waveNumber * waveNumber);

        Wave newWave;
        List<EnemyAttributes> allEnemies = EnemyLookup.Instance.enemies;
        List<EnemyAttributes> potentialEnemies = allEnemies.Where(enemyAttributes => (enemyAttributes.minWave <= waveNumber) && (enemyAttributes.maxWave >= waveNumber)).ToList();

        if (waveNumber == 0)
        {
            int randomIndex = Random.Range(0, potentialEnemies.Count);
            EnemyAttributes enemyPrefab = potentialEnemies[randomIndex];
            WaveEnemyAttributes waveAttributes = new WaveEnemyAttributes(enemyPrefab.gameObject, waveDifficulty / enemyPrefab.difficultyRating);

            newWave = new Wave(new List<WaveEnemyAttributes>() { waveAttributes }, (waveNumber % 3 > 0 ? 3 : 5));
            
            return newWave;
        }

        float minRange = Mathf.Lerp(0.0f, 1.0f, (float)waveNumber / numWaves);
        float enemyTypeRange = Random.Range(minRange, 1.0f);

        int numEnemyTypes = (int)Mathf.Lerp(MIN_ENEMY_TYPES * 100, MAX_ENEMY_TYPES * 100, enemyTypeRange);
        numEnemyTypes = Mathf.RoundToInt((float)numEnemyTypes / 100);


        List<WaveEnemyAttributes> waveEnemyAttributes = new();

        for (int i = 0; i < numEnemyTypes; i++)
        {
            int randomIndex = Random.Range(0, potentialEnemies.Count);

            EnemyAttributes enemyPrefab = potentialEnemies[randomIndex];
            potentialEnemies.Remove(enemyPrefab);

            waveEnemyAttributes.Add(new WaveEnemyAttributes(enemyPrefab.gameObject, waveDifficulty / numEnemyTypes / enemyPrefab.difficultyRating));
        }

        newWave = new Wave(waveEnemyAttributes, (waveNumber % 3 > 0 ? 3 : 5));
        return newWave;
    }
}
