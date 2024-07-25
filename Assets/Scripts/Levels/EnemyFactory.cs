using UnityEngine;
using UnityEngine.Splines;


public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }

    [SerializeField]
    SplineContainer trackSpline;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Trying to implement more that one {this}");
            Destroy(this);
        }

        Instance = this;
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefab);
        spawnedEnemy.GetComponent<SplineAnimate>().Container = trackSpline;
    }
}
