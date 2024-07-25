using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveParser))]
public class Level : MonoBehaviour
{
    public int numWaves { get; private set; } = 10;
    public int currentWaveNum { get; private set; } = 0;

    public List<Wave> waves { get; private set; } = new();
    WaveParser waveParser;

    private void Start()
    {
        waveParser = GetComponent<WaveParser>();

        GenerateWaves();
        AdvanceWave();
    }

    public void AdvanceWave()
    {
        currentWaveNum++;
        Wave currentWave = waves[currentWaveNum];
        waveParser.ParseWave(currentWave);
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
}
