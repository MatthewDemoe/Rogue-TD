using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyFactory 
{
    private static EnemyFactory _instance;
    public static EnemyFactory Instance 
    {
        get
        {
            if(_instance == null)
                _instance = new EnemyFactory();

            return _instance;
        }
    }

    /*
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Trying to implement more than one {this}");
            Destroy(this);
        }

        Instance = this;
    }
    */

    public GameObject SpawnEnemy(GameObject enemyPrefab, SplineContainer trackSpline)
    {
        GameObject spawnedEnemy = GameObject.Instantiate(enemyPrefab);
        spawnedEnemy.GetComponent<SplineAnimate>().Container = trackSpline;

        return spawnedEnemy;
    }
}
