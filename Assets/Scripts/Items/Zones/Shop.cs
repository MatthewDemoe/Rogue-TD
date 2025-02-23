using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : ItemZone
{
    [SerializeField]
    GameObject towerSlotParent;

    List<ItemSlot> towerSlots = new();

    public override Zone zone => Zone.Shop;

    void Start()
    {
        towerSlots = towerSlotParent.GetComponentsInChildren<ItemSlot>().ToList(); ;

        GenerateTowers();
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

    public override bool TryPlacement(HoldableItem item)
    {
        if (item.currentZone == zone)
            return false;

        base.TryPlacement(item);

        item.RemoveFromHoldingSlot();
        SellItem(item);

        return true;
    }

    private void SellItem(HoldableItem item)
    {
        PlayerProperties.Instance.AdjustMoney(item.sellValue);
        Destroy(item.gameObject);
    }
}
