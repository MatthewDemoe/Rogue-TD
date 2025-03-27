using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.PackageManager;

public class Shop : ItemZone
{
    [SerializeField]
    GameObject towerSlotParent;

    List<ItemSlot> towerSlots = new();

    public override Zone zone => Zone.Shop;

    protected override void Start()
    {
        base.Start();

        towerSlots = towerSlotParent.GetComponentsInChildren<ItemSlot>().ToList();

        GenerateTowers();
    }

    public void GenerateTowers()
    {
        List<GameObject> towers = TowerLookup.Instance.towers;

        for (int i = 0; i < towerSlots.Count; i++)
        {
            GameObject towerInstance = Instantiate(towers[Random.Range(0, towers.Count)], towerSlots[i].transform.position, Quaternion.identity);
            Tower tower = towerInstance.GetComponent<Tower>();
            tower.currentZone = zone;

            towerSlots[i].AddItem(tower);
        }
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

        item.RemoveFromHoldingSlot();
        SellItem(item);

        return true;
    }

    private void SellItem(HoldableItem item)
    {
        PlayerProperties.Instance.AdjustMoney(item.sellValue);
        Destroy(item.gameObject);
    }

    protected override void CheckActiveInNewState(GameStateTracker.GameState newState)
    {
        base.CheckActiveInNewState(newState);

        bool shouldBeActive = newState == GameStateTracker.GameState.Shop;

        foreach (Collider collider in zoneColliders)
        {
            collider.enabled = shouldBeActive;
        }
    }
}
