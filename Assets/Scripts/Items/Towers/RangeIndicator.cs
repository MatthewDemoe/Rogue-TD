using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField]
    private SphereCollider m_rangeCollider;

    [SerializeField]
    DecalProjector m_rangeIndicator;

    TowerProperties towerProperties;

    private void Awake()
    {
        towerProperties = GetComponent<TowerProperties>();

        AdjustRangeIndicator(TowerProperties.PropertyType.Range, towerProperties.range);

        towerProperties.OnPropertyChanged.AddListener(AdjustRangeIndicator);
    }

    private void AdjustRangeIndicator(TowerProperties.PropertyType propertyType, float newValue)
    {
        if(propertyType != TowerProperties.PropertyType.Range)
            return;

        m_rangeCollider.radius = newValue;
        m_rangeIndicator.size = Vector3.one * newValue;
    }
}
