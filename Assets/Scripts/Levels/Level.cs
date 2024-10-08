using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class Level : MonoBehaviour
{
    public static Level Instance { get; private set; }

    public int numWaves { get; private set; } = 10;
    public int currentWaveNum { get; private set; } = 0;

    public List<Wave> waves { get; private set; } = new();

    List<GameObject> aliveEnemies = new();

    [SerializeField]
    SplineContainer trackSpline;

    [SerializeField]
    private UnityEvent OnWaveStart = new();

    public UnityEvent OnWaveStarted { get { return OnWaveStart; } }

    [SerializeField]
    private UnityEvent OnWaveComplete = new();

    public UnityEvent OnWaveCompleted { get { return OnWaveComplete; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Trying to implement more than one {this}");
            Destroy(this);
        }

        Instance = this;
    }

    private void Start()
    {
        /*
        PlayerProperties.Instance.incomeSources.AddIncomeSource(new IncomeSource
            (
                sourceName: "Wave Income",
                amount: () => { return currentWaveNum % 3 > 0 ? 3 : 5; }
            ));
        */
        GenerateWaves();
    }

    public void StartWave()
    {
        currentWaveNum++;
        Wave currentWave = waves[currentWaveNum];
        ParseWave(currentWave);
        currentWave.Start();
        OnWaveStart.Invoke();
    }

    private void GenerateWaves()
    {
        for (int i = 0; i < numWaves; i++)
        {
            Wave wave = new Wave( new List<WaveAttributes>() { new WaveAttributes(EnemyLookup.Instance.hardEnemies[0], i) }, (i % 3 > 0 ? 3 : 5));
            //wave.waveAttributes.Add();
            waves.Add(wave);
        }
    }

    private void CreateEnemy(GameObject enemyToSpawn)
    {
        GameObject enemyInstance = Instantiate(enemyToSpawn);
        enemyInstance.GetComponent<SplineAnimate>().Container = trackSpline;

        EnemyActions enemyActions = enemyInstance.GetComponent<EnemyActions>();
        enemyActions.OnExited.AddListener(() => RemoveEnemy(enemyInstance));
        enemyActions.OnKilled.AddListener(() => RemoveEnemy(enemyInstance));

        aliveEnemies.Add(enemyInstance);
    }

    private void RemoveEnemy(GameObject removedEnemy)
    {
        aliveEnemies.Remove(removedEnemy);

        if (!aliveEnemies.Any())
            OnWaveComplete.Invoke();
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
            CreateEnemy(waveAttributes.waveEnemy);
            yield return new WaitForSeconds(enemyAttributes.spawnInterval);
        }
    }
}
