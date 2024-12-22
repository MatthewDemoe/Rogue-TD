using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject towerSlotParent;

    List<ItemSlot> itemSlots = new();

    [SerializeField]
    GameObject towerPrefab;

    Zone zone = Zone.Inventory;

    private void Start()
    {
        itemSlots = towerSlotParent.GetComponentsInChildren<ItemSlot>().ToList();

        TryAddItem(Instantiate(towerPrefab, itemSlots[0].transform.position, Quaternion.identity).GetComponent<HoldableItem>());
    }

    private static Inventory instance = null;
    public static Inventory Instance
    {
        get
        { 
            if (instance == null)
                instance = new GameObject().AddComponent<Inventory>();

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    public bool TryAddItem(HoldableItem holdableItem)
    {
        if (holdableItem.currentZone == zone)
            return false;

        ItemSlot firstEmptySlot = itemSlots.FirstOrDefault(slot => slot.heldItem is null);

        if (firstEmptySlot is null)
            return false;

        bool canBuyTower = false;
        if (holdableItem.currentZone == Zone.Shop)
        {
            canBuyTower = PlayerActions.Instance.CanBuyItem(holdableItem);
            print(canBuyTower);
            if (!canBuyTower)
                return false;

            PlayerActions.Instance.TryBuyItem(holdableItem);
        }        

        firstEmptySlot.AddItem(holdableItem);
        holdableItem.currentZone = zone;

        return true;
    }
}
