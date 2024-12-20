using System.Collections;
using UnityEngine;

public enum Zone { Empty, Shop, Inventory, Track }

public abstract class HoldableItem : MonoBehaviour
{
    private bool isHoldable = true;
    private bool isHeld = false;

    private float returnDuration = 0.5f;

    private ItemSlot holdingSlot = null;

    public Zone currentZone { get; set; } = Zone.Empty;

    public Vector3 lastPlacement = Vector3.zero;

    protected virtual void FixedUpdate()
    {
        if (isHeld)
            FollowMousePosition();
    }

    public bool TryHoldStarted()
    {
        if (!isHoldable)
            return false;

        SetFollowMousePosition(true);
        return true;
    }

    public void HoldEnded()
    {
        SetFollowMousePosition(false);
        CheckPlacement();
    }

    private void SetFollowMousePosition(bool state)
    {
        isHeld = state;
    }

    private void FollowMousePosition()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.y = 0.0f;

        transform.position = newPosition;
    }

    protected virtual void CheckPlacement()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        bool isColliding = Physics.BoxCast(transform.position - Vector3.down, boxCollider.bounds.extents, Vector3.down, out var hitInfo, Quaternion.identity, Mathf.Infinity);

        if (!isColliding)
        {
            if (currentZone == Zone.Inventory)
                ReturnToHoldingSlot();

            else
                ReturnToLastPlacement();
        }

        if (isColliding)
        {
            if (hitInfo.collider.tag == Tags.INVENTORY)
            {
                Inventory inventory = hitInfo.collider.GetComponentInParent<Inventory>();

                PlaceInInventory(inventory);
            }

            else if(hitInfo.collider.tag == Tags.TRACK)
                PlaceOnTrack();      
        }
    }

    public void PlaceInInventory(Inventory inventory)
    {
        if (currentZone == Zone.Inventory)
        {
            ReturnToHoldingSlot();
            return;
        }

        if (inventory.TryAddItem(this))
            currentZone = Zone.Inventory;
    }

    public void PlaceOnTrack()
    {
        if (currentZone == Zone.Inventory)
            RemoveFromHoldingSlot();

        currentZone = Zone.Track;
        lastPlacement = transform.position;
    }

    public void RemoveFromHoldingSlot()
    {    
        holdingSlot.RemoveItem();
        holdingSlot = null;
    }

    public void AddToItemSlot(ItemSlot itemSlot)
    {
        holdingSlot = itemSlot;
        ReturnToHoldingSlot();
    }

    private void ReturnToHoldingSlot()
    {
        StartCoroutine(ReturnToHoldingSlotRoutine());
    }

    private void ReturnToLastPlacement()
    {
        StartCoroutine(ReturnToLastPlacementRoutine());
    }

    private IEnumerator ReturnToHoldingSlotRoutine()
    {
        isHoldable = false;

        float timer = 0.0f;
        float mappedTimer = 0.0f;

        Vector3 startPosition = transform.position;

        while (timer < returnDuration)
        {
            mappedTimer = UtilMath.Lmap(timer, 0.0f, returnDuration, 0.0f, 1.0f);
            mappedTimer = UtilMath.EasingFunction.EaseInOutCirc(0.0f, 1.0f, mappedTimer);

            transform.position = Vector3.Lerp(startPosition, holdingSlot.transform.position, mappedTimer);

            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
        }

        isHoldable = true;
    }

    private IEnumerator ReturnToLastPlacementRoutine()
    {
        isHoldable = false;

        float timer = 0.0f;
        float mappedTimer = 0.0f;

        Vector3 startPosition = transform.position;

        while (timer < returnDuration)
        {
            mappedTimer = UtilMath.Lmap(timer, 0.0f, returnDuration, 0.0f, 1.0f);
            mappedTimer = UtilMath.EasingFunction.EaseInOutCirc(0.0f, 1.0f, mappedTimer);

            transform.position = Vector3.Lerp(startPosition, lastPlacement, mappedTimer);

            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
        }

        isHoldable = true;
    }
}