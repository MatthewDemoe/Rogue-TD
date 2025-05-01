using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    Button infoButton;

    [SerializeField]
    GameObject infoParent;

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
        infoButton.onClick.AddListener(ToggleDisplayingInfo);
        PlayerActions.Instance.OnLeftClick.AddListener(HideDisplayInfo);
    }

    private void HideDisplayInfo()
    {
        displayingInfo = false;
    }

    public void ToggleDisplayingInfo()
    {
        displayingInfo = !displayingInfo;
    }

    public void SetDisplayingInfo(bool state)
    {
        displayingInfo = state;
    }

    private void OnDestroy()
    {
        PlayerActions.Instance.OnLeftClick.RemoveListener(HideDisplayInfo);
    }
}
