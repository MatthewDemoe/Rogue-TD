using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    Button infoButton;

    [SerializeField]
    GameObject infoParent;

    [SerializeField]
    TextMeshProUGUI towerCost;

    [SerializeField]
    TextMeshProUGUI infoTowerName;

    [SerializeField]
    TextMeshProUGUI description;

    bool displayingInfo = false;

    private void Awake()
    {
        HoldableItem tower = GetComponentInParent<HoldableItem>();

        infoButton.onClick.AddListener(ToggleDisplayingInfo);

        SetTower(tower);

        PlayerActions.Instance.OnLeftClick.AddListener(HideDisplayInfo);
    }

    public void SetTower(HoldableItem tower)
    {
        infoTowerName.text = tower.itemName;
        towerCost.text = $"${tower.cost}"; 

        description.text = tower.description;
    }

    private void HideDisplayInfo()
    {
        displayingInfo = false;
        infoParent.gameObject.SetActive(displayingInfo);
    }

    public void ToggleDisplayingInfo()
    {
        displayingInfo = !displayingInfo;

        infoParent.gameObject.SetActive(displayingInfo);
    }

    public void SetDisplayingInfo(bool state)
    {
        displayingInfo = state;

        infoParent.gameObject.SetActive(state);
    }

    private void OnDestroy()
    {
        PlayerActions.Instance.OnLeftClick.RemoveListener(HideDisplayInfo);
    }
}
