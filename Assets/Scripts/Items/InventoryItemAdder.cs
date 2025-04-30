using UnityEngine;

public class InventoryItemAdder : MonoBehaviour
{
    [SerializeField]
    GameObject itemToAdd;

    public void AddItemToInventory()
    {
        if (!itemToAdd.TryGetComponent(out HoldableItem _))
            return;

        if(!Inventory.Instance.firstEmptySlot)
        {
            Debug.Log("No empty slots available in inventory.");
            return;
        }

        Vector3 itemSlotPosition = transform.position;

        GameObject itemInstance = Instantiate(itemToAdd, itemSlotPosition, Quaternion.identity);
        Inventory.Instance.TryPlacement(itemInstance.GetComponent<HoldableItem>());
    }
}
