using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public HoldableItem heldItem { get; private set; } = null;

    public void AddItem(HoldableItem item)
    {
        heldItem = item;
        item.AddToItemSlot(this);
    }

    public void RemoveItem()
    {
        heldItem = null;
    }
}
