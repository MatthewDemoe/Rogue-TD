using UnityEngine;

public class TowerSpreadProjectileAttacker : TowerProjectileAttacker
{
    [SerializeField]
    int numProjectiles = 5;

    [SerializeField]
    int degreeSpread = 30;

    protected override void Attack()
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            projectileInstance.GetComponent<Projectile>().Initialize(towerProperties);

            float angle = (i - (numProjectiles - 1) / 2) * degreeSpread;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * (towerProperties.enemyTracker.GetTarget().gameObject.transform.position - transform.position);
            projectileInstance.GetComponent<StraightProjectileMover>().SetDirection(direction.normalized);
        }        
    }
}
