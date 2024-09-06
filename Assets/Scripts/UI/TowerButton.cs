using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    GameObject towerPrefab;

    [SerializeField]
    TextMeshProUGUI towerName;

    [SerializeField]
    TextMeshProUGUI towerCost;

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

        towerName.text = towerProperties.towerName;
        towerCost.text = towerProperties.cost.ToString();
    }

    public void BuyTower()
    {     
        PlayerActions.Instance.TryBuyTower(towerPrefab);
    }
}
