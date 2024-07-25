using System.Collections;
using UnityEngine;

public class WaveParser : MonoBehaviour
{
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

            EnemyFactory.Instance.SpawnEnemy(waveAttributes.waveEnemy);
            yield return new WaitForSeconds(enemyAttributes.spawnInterval);
        }
    }
}
