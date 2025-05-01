using UnityEngine;
using TMPro;

public class HoldableItemInfo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI towerCost;

    [SerializeField]
    TextMeshProUGUI infoTowerName;

    [SerializeField]
    TextMeshProUGUI description;

    void Start()
    {
        HoldableItem item = GetComponentInParent<HoldableItem>();
        SetItemInfo(item);
    }

    public void SetItemInfo(HoldableItem item)
    {
        infoTowerName.text = item.itemName;
        towerCost.text = $"${item.cost}";

        description.text = item.description;
    }
}
