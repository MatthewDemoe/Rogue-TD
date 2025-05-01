using UnityEngine;

public class RangeIndicatorDisplayer : MonoBehaviour
{
    [SerializeField]
    TowerProperties towerProperties;

    public void SetRangeDecalActive(bool state)
    {
        if (towerProperties.currentZone != ItemZone.Zone.Track)
            gameObject.SetActive(false);

        else
            gameObject.SetActive(state);
    }
}
