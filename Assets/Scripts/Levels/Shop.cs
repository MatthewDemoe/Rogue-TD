using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    GameObject towerButtonParent;

    [SerializeField]
    GameObject towerButtonPrefab;

    void Start()
    {
        GenerateTowerButtons();
    }

    public void GenerateTowerButtons()
    {
        GameObject tower = null;
        AsyncOperationHandle<IList<GameObject>> loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            new List<string>() { "tower" },
            addressable =>
            {
                tower = addressable;
            }, Addressables.MergeMode.Intersection,
            false);

        loadHandle.WaitForCompletion();

        for (int i = 0; i < 3; i++)
        {
            GameObject towerButtonInstance = Instantiate(towerButtonPrefab, towerButtonParent.transform);
            towerButtonInstance.GetComponent<TowerButton>().SetTower(tower);
        }

        Addressables.Release(loadHandle);
    }

    public void ClearTowerButtons()
    {
        foreach (Transform child in towerButtonParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
