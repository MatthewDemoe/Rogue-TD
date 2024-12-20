using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    List<ItemSlot> itemSlots = new();

    [SerializeField]
    GameObject towerPrefab;

    Zone zone = Zone.Inventory;

    private void Start()
    {
        TryAddItem(Instantiate(towerPrefab).GetComponent<HoldableItem>());
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

        firstEmptySlot.AddItem(holdableItem);
        holdableItem.currentZone = zone;

        return true;
    }
}
