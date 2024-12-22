using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
    [SerializeField]
    GameObject towerSlotParent;

    List<ItemSlot> towerSlots = new();

    Zone zone = Zone.Shop;

    void Start()
    {
        towerSlots = towerSlotParent.GetComponentsInChildren<ItemSlot>().ToList(); ;

        GenerateTowers();

        PlayerActions.Instance.OnLeftClick.AddListener(DeselectTowers);
    }

    public void DeselectTowers()
    {
        towerSlotParent.GetComponentsInChildren<TowerButton>().ToList().ForEach(button => button.SetDisplayingInfo(false));
    }

    public void GenerateTowers()
    {
        GameObject towerPrefab = null;
        AsyncOperationHandle<IList<GameObject>> loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            new List<string>() { "tower" },
            addressable =>
            {
                towerPrefab = addressable;
            }, Addressables.MergeMode.Intersection,
            false);

        loadHandle.WaitForCompletion();

        for (int i = 0; i < towerSlots.Count; i++)
        {
            GameObject towerInstance = Instantiate(towerPrefab, towerSlots[i].transform.position, Quaternion.identity);
            Tower tower = towerInstance.GetComponent<Tower>();
            tower.currentZone = zone;

            towerSlots[i].AddItem(tower);
        }

        Addressables.Release(loadHandle);
    }

    public void ClearShopTowers()
    {
        towerSlots.ForEach(towerSlot => 
        {
            towerSlot.DestroyItem();
        });
    }
}
