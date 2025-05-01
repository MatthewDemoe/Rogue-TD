using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MechanicReferences))]
public class TowerProperties : HoldableItem
{
    [SerializeField]
    private float m_range = 3.0f;

    [SerializeField]
    private SphereCollider m_rangeCollider;

    public float range { get { return m_range; } }

    [SerializeField]
    private float m_fireRate = 1.0f;

    public float fireRate { get {return m_fireRate;} }

    [SerializeField]
    private float m_damage = 1.0f;

    public float damage { get { return m_damage; } }

    [SerializeField]
    private float m_duration = 1.0f;

    public float duration { get { return m_duration; } }

    public TowerEnemyTracker enemyTracker { get; private set; } = null;

    void Start()
    {
        enemyTracker = GetComponentInChildren<TowerEnemyTracker>();
        m_rangeCollider.radius = range;
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

    public void BoostStat(MechanicReferences.MechanicReference statToBoost)
    {
        switch (statToBoost)
        {
            case MechanicReferences.MechanicReference.Range:
                Debug.Log("Boosting range");
                break;
            case MechanicReferences.MechanicReference.FireRate:
                m_fireRate += 1.0f;
                break;

            default:
                Debug.LogWarning($"Stat {statToBoost} not found");
                break;
        }
    }
}
