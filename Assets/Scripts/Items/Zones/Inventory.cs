using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : ItemZone
{
    [SerializeField]
    GameObject towerSlotParent;

    List<ItemSlot> itemSlots = new();

    [SerializeField]
    GameObject towerPrefab;

    public override Zone zone => Zone.Inventory;

    protected override void Start()
    {
        base.Start();

        itemSlots = towerSlotParent.GetComponentsInChildren<ItemSlot>().ToList();

        TryPlacement(Instantiate(towerPrefab, itemSlots[0].transform.position, Quaternion.identity).GetComponent<HoldableItem>());
    }

    public override bool TryPlacement(HoldableItem item)
    {
        if (item.currentZone == zone)
            return false;

        ItemSlot firstEmptySlot = itemSlots.FirstOrDefault(slot => slot.heldItem is null);

        if (firstEmptySlot is null)
            return false;

        base.TryPlacement(item);

        bool canBuyTower = false;
        if (item.currentZone == Zone.Shop)
        {
            canBuyTower = PlayerActions.Instance.CanBuyItem(item);
            
            if (!canBuyTower)
                return false;

            PlayerActions.Instance.TryBuyItem(item);
        }

        firstEmptySlot.AddItem(item);
        item.currentZone = zone;

        return true;
    }


}
