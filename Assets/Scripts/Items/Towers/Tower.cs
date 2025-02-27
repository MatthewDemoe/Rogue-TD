using System.Linq;
using UnityEngine;

public class Tower : HoldableItem
{
    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float m_range = 3.0f;

    [SerializeField]
    private SphereCollider m_rangeCollider;

    public float range { get { return m_range; } }

    [SerializeField]
    private float m_fireRate = 1.0f;

    public float fireRate { get {return m_fireRate;} }

    private float timeSinceLastFire = 0.0f;

    [SerializeField]
    private float m_damage = 1.0f;

    public float damage { get { return m_damage; } }

    TowerEnemyTracker enemyTracker = null;

    [SerializeField]
    Texture2D towerSprite;

    void Start()
    {
        GetComponentInChildren<MeshRenderer>().material.mainTexture = towerSprite;

        m_rangeCollider.radius = range;
        enemyTracker = GetComponentInChildren<TowerEnemyTracker>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        TryShoot();
    }

    private void TryShoot()
    {
        timeSinceLastFire += Time.fixedDeltaTime;

        if (!enemyTracker.enemiesInRange.Any() || (timeSinceLastFire < fireRate))
            return;

        timeSinceLastFire = 0.0f;

        GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().Initialize(this);
    }

    public EnemyAttributes GetTarget()
    {
        return enemyTracker.enemiesInRange.OrderBy(enemy => enemy.distance).Last();
    }
}
