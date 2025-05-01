using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(MechanicReferences))]
public class TowerProperties : HoldableItem
{
    public enum PropertyType
    {
        Damage,
        FireRate, 
        Range,
        Duration, 
    }

    [SerializeField]
    private float m_baseRange = 3.0f;

    private float m_rangeBonus = 0.0f;

    public float rangeBonus 
    { 
        get { return m_rangeBonus; }

        set
        {
            m_rangeBonus = value;
            OnPropertyChanged.Invoke(PropertyType.Range, range);
        }
    }

    public float range => m_baseRange + rangeBonus;

    [SerializeField]
    private float m_fireRate = 1.0f;

    private float m_fireRateBonus = 0.0f;

    public float fireRateBonus 
    {
        get { return m_fireRateBonus; }
        set
        {
            m_fireRateBonus = value;
            OnPropertyChanged.Invoke(PropertyType.FireRate, fireRate);
        }
    }

    public float fireRate => m_fireRate + fireRateBonus;

    [SerializeField]
    private float m_damage = 1.0f;

    private float m_damageBonus = 0.0f;

    public float damageBonus
    {
        get { return m_damageBonus; }
        set
        {
            m_damageBonus = value;
            OnPropertyChanged.Invoke(PropertyType.Damage, damage);
        }
    }

    public float damage => m_damage + damageBonus; 

    [SerializeField]
    private float m_duration = 1.0f;

    private float m_durationBonus = 0.0f;

    public float durationBonus
    {
        get { return m_durationBonus; }
        set
        {
            m_durationBonus = value;
            OnPropertyChanged.Invoke(PropertyType.Duration, duration);
        }
    }

    public float duration => m_duration + durationBonus; 

    public TowerEnemyTracker enemyTracker { get; private set; } = null;

    [SerializeField]
    private UnityEvent<PropertyType, float> m_OnRangeChanged = new();

    public UnityEvent<PropertyType, float> OnPropertyChanged => m_OnRangeChanged;

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

    public void BoostStat(MechanicReferences.MechanicReference statToBoost, float bonusAmount)
    {
        switch (statToBoost)
        {
            case MechanicReferences.MechanicReference.Damage:
                damageBonus += bonusAmount;
                break;

            case MechanicReferences.MechanicReference.Fire:
                damageBonus += bonusAmount;
                break;

            case MechanicReferences.MechanicReference.Poison:
                damageBonus += bonusAmount;
                break;

            case MechanicReferences.MechanicReference.Slow:
                durationBonus += bonusAmount;
                break;

            case MechanicReferences.MechanicReference.Range:
                rangeBonus += bonusAmount;
                break;

            case MechanicReferences.MechanicReference.FireRate:
                fireRateBonus += bonusAmount;
                break;

            default:
                Debug.LogWarning($"Stat {statToBoost} not found");
                break;
        }
    }
}
