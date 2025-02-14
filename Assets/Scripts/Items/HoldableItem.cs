using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class HoldableItem : MonoBehaviour
{
    [SerializeField]
    string m_itemName = string.Empty;
    public string itemName { get { return m_itemName; } }

    [SerializeField]
    [TextArea]
    string m_description = string.Empty;

    public string description { get { return m_description; } }

    [SerializeField]
    private int m_cost = 2;
    public int cost { get { return m_cost; } }

    public int sellValue => cost / 2;

    private bool isHoldable = true;
    private bool isHeld = false;

    private float returnDuration = 0.5f;

    private ItemSlot holdingSlot = null;

    public ItemZone.Zone currentZone { get; set; } = ItemZone.Zone.Empty;

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

        RaycastHit[] allHits = Physics.BoxCastAll(transform.position - Vector3.down, boxCollider.bounds.extents, Vector3.down, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask(new List<string>() { "Track", "ItemZone" }.ToArray()));
        bool isColliding = allHits.Any();
        bool isCollidingWithTrack = allHits.Any((hit) => hit.collider.TryGetComponent(out SplineSampler _));

        if (!isColliding || isCollidingWithTrack)
        {
            ReturnToProperLocation();
            return;
        }

        RaycastHit hitInfo = allHits.First((hit) => hit.collider.TryGetComponent(out ItemZone newZone));

        if (hitInfo.collider.TryGetComponent(out ItemZone newZone))
        {
            bool placedSuccessfully = newZone.TryPlacement(this);

            if (!placedSuccessfully)
                ReturnToProperLocation();
        }
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

    private void ReturnToProperLocation()
    {
        if (currentZone == ItemZone.Zone.Inventory || currentZone == ItemZone.Zone.Shop)
            ReturnToHoldingSlot();

        else
            ReturnToLastPlacement();
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