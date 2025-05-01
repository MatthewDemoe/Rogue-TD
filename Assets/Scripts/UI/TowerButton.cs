using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField]
    GameObject rangeDecal;

    bool m_displayingInfo = false;

    public bool displayingInfo
    {
        get => m_displayingInfo;
        set
        {
            m_displayingInfo = value;
            OnDisplayingInfoChanged.Invoke(m_displayingInfo);
        }
    }

    [SerializeField]
    private UnityEvent<bool> m_OnDisplayingInfoChanged = new();

    public UnityEvent<bool> OnDisplayingInfoChanged => m_OnDisplayingInfoChanged;

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
