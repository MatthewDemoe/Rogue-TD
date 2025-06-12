using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class LevelProperties : MonoBehaviour
{
    private static LevelProperties _instance = null;

    public static LevelProperties Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("LevelProperties");
                _instance = obj.AddComponent<LevelProperties>();               
            }

            return _instance;
        }
    }

    public int numWaves { get; private set; } = 15;
    public int currentWaveNum { get; private set; } = 0;

    List<GameObject> aliveEnemies = new();

    [SerializeField]
    SplineContainer trackSpline;

    WaveGenerator waveGenerator = null;

    public static UnityEvent OnWaveStart { get; } = new();

    public static UnityEvent OnWaveComplete { get; } = new();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        if (trackSpline == null)
        {
            trackSpline = FindFirstObjectByType<SplineContainer>();
            if (trackSpline == null)
            {
                Debug.LogError("No SplineContainer found in the scene.");
            }
        }
    }

    private void Start()
    {
        waveGenerator = new WaveGenerator(numWaves);

        OnWaveComplete.AddListener(() => currentWaveNum++);
        OnWaveComplete.AddListener(() => GameStateTracker.Instance.TrySetGameState(GameStateTracker.GameState.Track));
    }

    public void StartWave()
    {
        bool waveStarted = GameStateTracker.Instance.TrySetGameState(GameStateTracker.GameState.Level);

        if(!waveStarted)
            return;

        Wave currentWave = waveGenerator.waves[currentWaveNum];
        ParseWave(currentWave);

        currentWave.Start();
        OnWaveStart.Invoke();
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

    IEnumerator ParseWaveAttributes(WaveEnemyAttributes waveAttributes)
    {
        yield return new WaitForSeconds(waveAttributes.spawnDelay);
        EnemyAttributes enemyAttributes = waveAttributes.waveEnemy.GetComponent<EnemyAttributes>();
        waveAttributes.waveEnemy.GetComponent<SplineAnimate>().Container = trackSpline;

        int totalEnemies = (int)(waveAttributes.enemyAmount * enemyAttributes.spawnAmountMultiplier);

        for (int i = 0; i < totalEnemies; i++)
        {
            CreateEnemy(waveAttributes.waveEnemy);
            yield return new WaitForSeconds(enemyAttributes.spawnInterval);
        }
    }

    public Dictionary<EnemyAttributes, int> GetEnemyCounts()
    {
        Wave currentWave = waveGenerator.waves[currentWaveNum];
        Dictionary<EnemyAttributes, int> enemyCounts = new();

        currentWave.waveAttributes.ForEach(waveAttribute =>
        {
            EnemyAttributes enemyAttributes = waveAttribute.waveEnemy.GetComponent<EnemyAttributes>();           
            enemyCounts[enemyAttributes] = (int)(waveAttribute.enemyAmount * enemyAttributes.spawnAmountMultiplier);
        });

        return enemyCounts;
    }
}
