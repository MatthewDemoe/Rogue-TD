using System.Linq;
using UnityEngine;

public class TowerProjectileAttacker : TowerAttacker
{
    [SerializeField]
    protected GameObject projectile;

    protected override void Attack()
    {
        GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().Initialize(towerProperties);
    }
}
