using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int numWaves { get; private set; } = 10;
    public int currentWaveNum { get; private set; } = 0;

    public List<Wave> waves { get; private set; } = new();

    List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        GenerateWaves();
        AdvanceWave();
    }

    public void AdvanceWave()
    {
        currentWaveNum++;
        Wave currentWave = waves[currentWaveNum];
        ParseWave(currentWave);
    }

    private void GenerateWaves()
    {
        for (int i = 0; i < numWaves; i++)
        {
            Wave wave = new Wave();
            wave.waveAttributes.Add(new WaveAttributes(EnemyLookup.Instance.hardEnemies[0], i));
            waves.Add(wave);
        }
    }

    private void CreateEnemy(GameObject enemyToSpawn)
    {
        GameObject enemyInstance = EnemyFactory.Instance.SpawnEnemy(enemyToSpawn);

        EnemyActions enemyActions = enemyInstance.GetComponent<EnemyActions>();
        enemyActions.OnExited.AddListener(() => RemoveEnemy(enemyInstance));
        enemyActions.OnKilled.AddListener(() => RemoveEnemy(enemyInstance));

        aliveEnemies.Add(enemyInstance);
    }

    private void RemoveEnemy(GameObject removedEnemy)
    {
        aliveEnemies.Remove(removedEnemy);

        if (!aliveEnemies.Any())
        {
            AdvanceWave();
        }
    }

    public void ParseWave(Wave wave)
    {
        wave.waveAttributes.ForEach(waveAttribute => StartCoroutine(ParseWaveAttributes(waveAttribute)));
    }

    IEnumerator ParseWaveAttributes(WaveAttributes waveAttributes)
    {
        Debug.Log("Parsing Wave");

        yield return new WaitForSeconds(waveAttributes.spawnDelay);
        EnemyAttributes enemyAttributes = waveAttributes.waveEnemy.GetComponent<EnemyAttributes>();
        Debug.Log($"Creating {enemyAttributes.displayName}");

        int totalEnemies = (int)(waveAttributes.enemyAmount * enemyAttributes.spawnAmountMultiplier);

        for (int i = 0; i < totalEnemies; i++)
        {
            Debug.Log($"{i}");

            CreateEnemy(waveAttributes.waveEnemy);
            yield return new WaitForSeconds(enemyAttributes.spawnInterval);
        }
    }
}
