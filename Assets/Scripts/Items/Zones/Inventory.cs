using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : ItemZone
{
    public static Inventory Instance;

    [SerializeField]
    GameObject towerSlotParent;

    List<ItemSlot> itemSlots = new();

    public ItemSlot firstEmptySlot => itemSlots.FirstOrDefault(slot => slot.heldItem is null);

    [SerializeField]
    GameObject towerPrefab;

    public override Zone zone => Zone.Inventory;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

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

        if (firstEmptySlot is null)
            return false;

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
