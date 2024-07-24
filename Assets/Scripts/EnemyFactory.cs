using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Splines;
using Lookups;


public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    SplineContainer trackSpline;

    List<string> enemyTags = new List<string>() { AddressableLabels.enemy, AddressableLabels.easy };
    public List<GameObject> enemyPrefabs { get; private set; } = new List<GameObject>();

    AsyncOperationHandle<IList<GameObject>> loadHandle;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadEnemies());
    }

    private IEnumerator LoadEnemies()
    {
        loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            enemyTags,
            addressable =>
            {
                enemyPrefabs.Add(addressable);                
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail and release if any asset fails to load

        yield return new WaitUntil(() => loadHandle.IsDone);

        enemyPrefabs.ForEach(enemy => SpawnEnemy(enemy));
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefab);
        spawnedEnemy.GetComponent<SplineAnimate>().Container = trackSpline;
    }

    private void OnDestroy()
    {
        Addressables.Release(loadHandle);
    }
}
