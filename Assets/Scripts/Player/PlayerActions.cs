using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
        bool hitPlayArea = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out var _, Mathf.Infinity, LayerMask.GetMask("Default"));

        if (PlayerProperties.Instance.isHoldingTower && hitPlayArea)
            TryPlaceTower();

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

    public void OnRMB(InputAction.CallbackContext context)
    {
        Debug.Log($"OnRMB");
    }

    public bool TryBuyTower(GameObject towerPrefab)
    {
        Tower towerProperties = towerPrefab.GetComponent<Tower>();

        if (PlayerProperties.Instance.money < towerProperties.cost || PlayerProperties.Instance.isHoldingTower)
            return false;

        PlayerProperties.Instance.AdjustMoney(-towerProperties.cost);

        PlayerProperties.Instance.heldTower = Instantiate(towerPrefab);
        towerProperties = PlayerProperties.Instance.heldTower.GetComponent<Tower>();
        towerProperties.SetFollowMousePosition(true);

        PlayerProperties.Instance.isHoldingTower = true;

        return true;
    }

    public bool TryPlaceTower()
    {
        Collider mainCollider = PlayerProperties.Instance.heldTower.GetComponents<Collider>().ToList().First(collider => !collider.isTrigger);

        bool isColliding = Physics.BoxCast(PlayerProperties.Instance.heldTower.transform.position - Vector3.forward, mainCollider.bounds.extents, Vector3.forward, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("Track"));

        if (isColliding) //Placeholder - return when placed over track. 
            return false;

        PlayerProperties.Instance.heldTower.GetComponent<Tower>().SetFollowMousePosition(false);
        PlayerProperties.Instance.heldTower = null;
        PlayerProperties.Instance.isHoldingTower = false;

        return true;
    }

}
