using System.Collections.Generic;
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


    List<EnemyAttributes> enemiesInRange = new();

    void Start()
    {
        m_rangeCollider.radius = range;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        TryShoot();
    }

    private void TryShoot()
    {
        timeSinceLastFire += Time.fixedDeltaTime;

        if (!enemiesInRange.Any() || (timeSinceLastFire < fireRate))
            return;

        print("Firing");
        timeSinceLastFire = 0.0f;

        //EnemyAttributes targetEnemy = GetTarget();
        //targetEnemy.TakeDamage(damage);

        GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().Initialize(this);
    }

    public EnemyAttributes GetTarget()
    {
        return enemiesInRange.OrderBy(enemy => enemy.distance).Last();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAttributes enemy = other.gameObject.GetComponent<EnemyAttributes>();

        enemiesInRange.Add(enemy);
        other.gameObject.GetComponent<EnemyActions>().OnKilled.AddListener(() => enemiesInRange.Remove(enemy));
        other.gameObject.GetComponent<EnemyActions>().OnExited.AddListener(() => enemiesInRange.Remove(enemy));

        print($"{other.gameObject} entered");
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject.GetComponent<EnemyAttributes>());

        print($"{other.gameObject} exited");
    }
}
