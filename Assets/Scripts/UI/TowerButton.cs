using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    Button infoButton;

    [SerializeField]
    Button buyButton;

    [SerializeField]
    GameObject towerPrefab;

    [SerializeField]
    GameObject infoParent;

    [SerializeField]
    TextMeshProUGUI mainTowerName;

    [SerializeField]
    TextMeshProUGUI towerCost;

    [SerializeField]
    TextMeshProUGUI infoTowerName;

    [SerializeField]
    TextMeshProUGUI range;

    [SerializeField]
    TextMeshProUGUI damage;

    [SerializeField]
    TextMeshProUGUI fireRate;

    [SerializeField]
    TextMeshProUGUI description;

    Tower towerProperties;

    bool displayingInfo = false;

    private void Awake()
    {
        buyButton.onClick.AddListener(BuyTower);
        buyButton.onClick.AddListener(ToggleDisplayingInfo);

        infoButton.onClick.AddListener(ToggleDisplayingInfo);

        if (towerPrefab != null)
            SetTower(towerPrefab);
    }

    public void SetTower(GameObject tower)
    {
        towerPrefab = tower;
        towerProperties = tower.GetComponent<Tower>();

        mainTowerName.text = towerProperties.towerName;
        infoTowerName.text = towerProperties.towerName;
        towerCost.text = towerProperties.cost.ToString();

        range.text = $"Range : {towerProperties.range}";
        damage.text = $"Damage : {towerProperties.damage}";
        fireRate.text = $"Fire Rate : {towerProperties.fireRate}";

        description.text = towerProperties.description;
    }

    public void BuyTower()
    {
        PlayerActions.Instance.TryBuyTower(towerPrefab);
    }

    public void ToggleDisplayingInfo()
    {
        displayingInfo = !displayingInfo;

        mainTowerName.gameObject.SetActive(!displayingInfo);
        infoParent.gameObject.SetActive(displayingInfo);
    }

    public void SetDisplayingInfo(bool state)
    {
        displayingInfo = state;

        mainTowerName.gameObject.SetActive(!state);
        infoParent.gameObject.SetActive(state);
    }
}
