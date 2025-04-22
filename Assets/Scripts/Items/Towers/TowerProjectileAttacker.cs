using System.Linq;
using UnityEngine;

public class TowerProjectileAttacker : TowerAttacker
{
    [SerializeField]
    GameObject projectile;

    protected override void TryAttack()
    {
        timeSinceLastAttack += Time.fixedDeltaTime;

        if (!towerProperties.enemyTracker.enemiesInRange.Any() || (timeSinceLastAttack < towerProperties.fireRate))
            return;

        timeSinceLastAttack = 0.0f;

        GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().Initialize(towerProperties);
    }
}
