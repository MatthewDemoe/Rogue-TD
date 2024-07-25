using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Lookups;

public class EnemyLookup 
{
    private static EnemyLookup m_instance = null;
    public static EnemyLookup Instance 
    {
        get
        {
            if(m_instance == null)
                m_instance = new EnemyLookup();

            return m_instance;
        }

        private set
        {
            m_instance = value;
        }
    }

    private List<string> easyEnemyTags = new List<string>() { AddressableLabels.enemy, AddressableLabels.easy };
    private List<string> mediumEnemyTags = new List<string>() { AddressableLabels.enemy, AddressableLabels.medium };
    private List<string> hardEnemyTags = new List<string>() { AddressableLabels.enemy, AddressableLabels.hard };

    public List<GameObject> easyEnemies { get; private set; } = new();
    public List<GameObject> mediumEnemies { get; private set; } = new();
    public List<GameObject> hardEnemies { get; private set; } = new();

    public EnemyLookup()
    {
        GetEnemies(easyEnemyTags, easyEnemies);
        GetEnemies(mediumEnemyTags, mediumEnemies);
        GetEnemies(hardEnemyTags, hardEnemies);
    }

    private void GetEnemies(List<string> tags, List<GameObject> enemyList)
    {
        AsyncOperationHandle<IList<GameObject>> loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            tags,
            addressable =>
            {
                enemyList.Add(addressable);
            }, Addressables.MergeMode.Intersection,
            false);

        loadHandle.WaitForCompletion();
        Addressables.Release(loadHandle);
    }
}
