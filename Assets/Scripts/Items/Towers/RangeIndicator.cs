using System.Linq;
using Unity.Properties;
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

        NamedProperty rangeProperty = towerProperties.TryGetProperty(NamedProperty.PropertyType.Range);

        if (rangeProperty is null)
        {
            Debug.LogWarning($"Stat {rangeProperty} not found on tower.");
            Destroy(this);
            return;
        }

        AdjustRangeIndicator(rangeProperty);

        towerProperties.OnPropertyChanged.AddListener(AdjustRangeIndicator);
    }

    private void AdjustRangeIndicator(NamedProperty namedProperty)
    {
        if(namedProperty.propertyType != NamedProperty.PropertyType.Range)
            return;

        m_rangeCollider.radius = namedProperty.propertyValue;
        m_rangeIndicator.size = Vector3.one * namedProperty.propertyValue;
    }
}
