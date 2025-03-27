using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TowerLookup
{
    private static TowerLookup m_instance = null;
    public static TowerLookup Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new TowerLookup();

            return m_instance;
        }

        private set
        {
            m_instance = value;
        }
    }

    public List<GameObject> towers { get; private set; } = new();

    private TowerLookup()
    {
        GetTowers(new List<string>() { "tower" });
    }

    private void GetTowers(List<string> tags)
    {
        AsyncOperationHandle<IList<GameObject>> loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            tags,
            addressable =>
            {
                towers.Add(addressable);
            }, Addressables.MergeMode.Intersection,
            false);

        loadHandle.WaitForCompletion();
        Addressables.Release(loadHandle);
    }
}
