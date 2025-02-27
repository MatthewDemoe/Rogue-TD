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


    public List<EnemyAttributes> enemies { get; private set; } = new();

    public EnemyLookup()
    {
        GetEnemies(new List<string>() { AddressableLabels.enemy }, enemies);
    }

    private void GetEnemies(List<string> tags, List<EnemyAttributes> enemyList)
    {
        AsyncOperationHandle<IList<GameObject>> loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            tags,
            addressable =>
            {
                EnemyAttributes enemyAttributes = addressable.GetComponent<EnemyAttributes>();

                enemyList.Add(enemyAttributes);
            }, Addressables.MergeMode.Intersection,
            false);

        loadHandle.WaitForCompletion();
        Addressables.Release(loadHandle);
    }
}
