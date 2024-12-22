using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerActions : MonoBehaviour
{
    public static PlayerActions Instance { get; private set; } = null;

    private UnityEvent _OnLeftClick = new();
    public UnityEvent OnLeftClick { get { return _OnLeftClick; } }

    [SerializeField]
    GraphicRaycaster graphicRaycaster = null;

    [SerializeField]
    EventSystem eventSystem = null;
    PointerEventData eventData = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Trying to create more than one {this}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OnLMB(InputAction.CallbackContext context)
    {
        bool hitPlayArea = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out var _, Mathf.Infinity, LayerMask.GetMask("Default"));

        if (context.performed)
            LMBDown();

        else
            LMBUp();
        
        /*
        if (PlayerProperties.Instance.isHoldingTower && hitPlayArea)
            TryPlaceTower();
        */ 

        List<RaycastResult> results = new();

        eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;
        graphicRaycaster.Raycast(eventData, results);

        foreach(var hit in results)
        {
            Debug.Log(hit.gameObject.name);
        }
        bool hitUI = results.Any();//Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out var _, Mathf.Infinity, LayerMask.GetMask("UI"));

        if(!hitUI)
            OnLeftClick.Invoke();
    }

    private void LMBDown()
    {
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out var hitInfo, Mathf.Infinity, LayerMask.GetMask("Tower")))
        {
            Tower tower = hitInfo.collider.GetComponent<Tower>();

            PlayerProperties.Instance.HoldItem(tower);
        }
    }

    private void LMBUp()
    {
        if (PlayerProperties.Instance.isHoldingItem)
            PlayerProperties.Instance.DropItem();
    }

    public void OnRMB(InputAction.CallbackContext context)
    {
        Debug.Log($"OnRMB");
    }

    public bool CanBuyItem(HoldableItem item)
    {
        return PlayerProperties.Instance.money >= item.cost;
    }

    public bool TryBuyItem(HoldableItem item)
    {        
        if (PlayerProperties.Instance.money < item.cost)
            return false;

        PlayerProperties.Instance.AdjustMoney(-item.cost);        

        return true;
    }
}
