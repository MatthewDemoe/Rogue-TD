using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class HoldableItem : MonoBehaviour
{
    [SerializeField]
    private GameObject descriptionUI;

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

    [SerializeField]
    private UnityEvent m_OnSell = new();

    public UnityEvent OnSell => m_OnSell;

    public int sellValue => cost / 2;
    
    protected Vector3 lastPlacement = Vector3.zero;

    private bool isHoldable = true;
    private bool isHeld = false;

    private float returnDuration = 0.5f;
    private float initialMouseMoveThreshold = 0.5f;
    private float mouseDistance = 0.0f;
    private bool hasDoneInitialMove = false;

    private ItemSlot holdingSlot = null;

    private ItemZone.Zone _currentZone = ItemZone.Zone.Empty;
    public ItemZone.Zone currentZone 
    {
        get
        {
            return _currentZone;
        }
        set
        {
            _currentZone = value;
            CheckActiveInNewState(GameStateTracker.Instance.currentState);
        }
    }


    private void Awake()
    {
        if (name == string.Empty)
            name = GetType().Name;

        CheckActiveInNewState(GameStateTracker.Instance.currentState);
        GameStateTracker.Instance.OnGameStateChange.AddListener(CheckActiveInNewState);
    }

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
        hasDoneInitialMove = false;
    }

    private void SetFollowMousePosition(bool state)
    {
        isHeld = state;
    }

    private void FollowMousePosition()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.y = 0.0f;

        mouseDistance = Vector3.Distance(newPosition, transform.position);

        if(!hasDoneInitialMove && mouseDistance < initialMouseMoveThreshold)
            return;

        hasDoneInitialMove = true;
        descriptionUI.SetActive(false);
        transform.position = newPosition;
    }

    protected virtual void CheckPlacement()
    {
        descriptionUI.SetActive(true);

        BoxCollider boxCollider = GetComponent<BoxCollider>();

        RaycastHit[] allHits = Physics.BoxCastAll(transform.position - Vector3.down, boxCollider.bounds.extents, Vector3.down, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask(new List<string>() { "Track", "ItemZone" }.ToArray()));
        RaycastHit hitInfo = allHits.First((hit) => hit.collider.TryGetComponent(out ItemZone newZone));

        if (hitInfo.collider.TryGetComponent(out ItemZone newZone))
        {
            bool placedSuccessfully = newZone.TryPlacement(this);

            if (!placedSuccessfully)
            {
                ReturnToProperLocation();
                return;
            }

            lastPlacement = transform.position;
        }
    }

    public void RemoveFromHoldingSlot()
    {
        if (holdingSlot == null)
            return;

        holdingSlot.RemoveItem();
        holdingSlot = null;
    }
    
    public void SetZone(ItemZone itemZone)
    {
        currentZone = itemZone.zone;
        transform.parent = itemZone.transform;
    }   

    public void AddToItemSlot(ItemSlot itemSlot)
    {
        holdingSlot = itemSlot;      

        if(gameObject.activeInHierarchy)
            ReturnToHoldingSlot();
    }

    protected void ReturnToProperLocation()
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

    private void CheckActiveInNewState(GameStateTracker.GameState newState)
    {
        if (currentZone == ItemZone.Zone.Shop)
            gameObject.SetActive(newState == GameStateTracker.GameState.Shop);

        else if (currentZone == ItemZone.Zone.Track)
            gameObject.SetActive(newState != GameStateTracker.GameState.Shop);
    }
}