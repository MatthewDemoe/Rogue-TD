using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    GameObject towerPrefab;

    Tower towerProperties;

    private void Awake()
    {
        button.onClick.AddListener(BuyTower);

        if (towerPrefab != null)
            SetTower(towerPrefab);
    }

    public void SetTower(GameObject tower)
    {
        towerPrefab = tower;
        towerProperties = tower.GetComponent<Tower>();
    }

    public void BuyTower()
    {     
        PlayerActions.Instance.TryBuyTower(towerPrefab);
    }
}
