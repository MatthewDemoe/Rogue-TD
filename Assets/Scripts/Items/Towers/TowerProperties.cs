using System.Linq;
using UnityEngine;

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
}
