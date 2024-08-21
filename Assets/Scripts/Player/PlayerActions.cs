using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions
{
    private static PlayerActions _instance = null;

    public static PlayerActions Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerActions();

            return _instance;
        }
    }

    public bool TryBuyTower(GameObject towerPrefab)
    {
        Tower towerProperties = towerPrefab.GetComponent<Tower>();

        if (PlayerProperties.Instance.money < towerProperties.cost || PlayerProperties.Instance.isHoldingTower)
            return false;

        PlayerProperties.Instance.AdjustMoney(-towerProperties.cost);

        GameObject towerInstance = GameObject.Instantiate(towerPrefab);
        towerProperties = towerInstance.GetComponent<Tower>();
        towerProperties.SetFollowMousePosition(true);

        PlayerProperties.Instance.isHoldingTower = true;

        return true;
    }

    public bool TryPlaceTower(GameObject towerInstance)
    {
        return true;
    }
}
