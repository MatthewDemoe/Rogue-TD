using UnityEngine;
using System.Linq;  

public class TowerPositionAttacker : TowerAttacker
{
    [SerializeField]
    GameObject positionEffect;

    protected override void Attack()
    {
        GameObject positionEffectInstance = Instantiate(positionEffect, towerProperties.enemyTracker.GetTarget().gameObject.transform.position, Quaternion.identity);
        positionEffectInstance.GetComponents<ProjectileComponent>().ToList().ForEach(component => component.Initialize(towerProperties));
    }
}
