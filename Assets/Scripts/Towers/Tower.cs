using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float range = 3.0f;

    [SerializeField]
    private float fireRate = 1.0f;

    private float timeSinceLastFire = 0.0f;

    [SerializeField]
    private float m_damage = 1.0f;

    public float damage { get { return m_damage; } }

    [SerializeField]
    private int m_cost = 2;
    public int cost { get { return m_cost; } }

    private bool isFollowingMouse = false;

    List<EnemyAttributes> enemiesInRange = new();

    void Start()
    {
        GetComponent<SphereCollider>().radius = range;
    }


    private void FixedUpdate()
    {
        if (isFollowingMouse)
            FollowMousePosition();

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

        GameObject projectileInstance = Instantiate(projectile);
        projectileInstance.GetComponent<Projectile>().Initialize(this);
    }

    public EnemyAttributes GetTarget()
    {
        return enemiesInRange.OrderBy(enemy => enemy.distance).Last();
    }

    public void SetFollowMousePosition(bool state)
    {
        isFollowingMouse = state;
    }

    private void FollowMousePosition()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0.0f;

        transform.position = newPosition;
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
