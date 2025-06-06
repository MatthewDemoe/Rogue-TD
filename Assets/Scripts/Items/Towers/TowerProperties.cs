using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TowerProperties : HoldableItem
{
    [SerializeField]
    private List<NamedProperty> m_namedProperties;

    public List<NamedProperty> namedProperties => m_namedProperties;
    public NamedProperty TryGetProperty(NamedProperty.PropertyType property) => namedProperties.FirstOrDefault(namedProperty => namedProperty.propertyType == property);

    public TowerEnemyTracker enemyTracker { get; private set; } = null;

    [SerializeField]
    private UnityEvent<NamedProperty> m_OnPropertyChanged = new();

    public UnityEvent<NamedProperty> OnPropertyChanged => m_OnPropertyChanged;

    void Start()
    {
        enemyTracker = GetComponentInChildren<TowerEnemyTracker>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void CheckPlacement()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        RaycastHit[] allHits = Physics.BoxCastAll(transform.position - Vector3.down, boxCollider.bounds.extents, Vector3.down, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask(new List<string>() { "Track", "ItemZone" }.ToArray()));

        bool isColliding = allHits.Any();
        bool isCollidingWithTrack = allHits.Any((hit) => hit.collider.TryGetComponent(out SplineSampler _));

        if (!isColliding || isCollidingWithTrack)
        {
            ReturnToProperLocation();
            return;
        }

        base.CheckPlacement();
    }

    public void BoostStat(NamedProperty.PropertyType property, float bonusAmount)
    {
        NamedProperty propertyToBoost = namedProperties.FirstOrDefault(namedProperty => namedProperty.propertyType == property);

        if (propertyToBoost is null)
        {
            Debug.LogWarning($"Stat {property} not found on tower.");
            return;
        }

        propertyToBoost.propertyBonus += bonusAmount;
        OnPropertyChanged.Invoke(propertyToBoost);
    }
}
