using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public static PlayerActions Instance { get; private set; } = null;

    public UnityEvent OnPlayAreaClicked { get; private set; } = new();

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
        Debug.Log($"OnLMB");
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

        PlayerProperties.Instance.heldTower = GameObject.Instantiate(towerPrefab);
        towerProperties = PlayerProperties.Instance.heldTower.GetComponent<Tower>();
        towerProperties.SetFollowMousePosition(true);

        PlayerProperties.Instance.isHoldingTower = true;

        OnPlayAreaClicked.AddListener(TryPlaceTower);

        return true;
    }

    public void TryPlaceTower()
    {
        if (false) //Placeholder - return when placed over track. 
            return;

        PlayerProperties.Instance.heldTower.GetComponent<Tower>().SetFollowMousePosition(false);
        PlayerProperties.Instance.heldTower = null;
        PlayerProperties.Instance.isHoldingTower = false;
        OnPlayAreaClicked.RemoveListener(TryPlaceTower);
    }
}
