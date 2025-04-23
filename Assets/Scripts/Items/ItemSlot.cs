using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public ItemZone itemZone { get; private set; } = null;

    public HoldableItem heldItem { get; private set; } = null;

    private void Awake()
    {
        itemZone = GetComponentInParent<ItemZone>();
    }

    public void AddItem(HoldableItem item)
    {
        item.RemoveFromHoldingSlot();

        heldItem = item;

        item.AddToItemSlot(this);

        heldItem.currentZone = itemZone.zone;
        item.transform.parent = transform;
        item.transform.rotation = transform.rotation;
    }

    public void RemoveItem()
    {
        heldItem = null;
    }

    public void DestroyItem()
    {
        if(heldItem == null)
            return; 

        Destroy(heldItem.gameObject);
        heldItem = null;
    }
}
