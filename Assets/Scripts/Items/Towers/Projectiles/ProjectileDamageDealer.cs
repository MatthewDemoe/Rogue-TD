using UnityEngine;

public class ProjectileDamageDealer : ProjectileComponent
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyAttributes enemyAttributes))
            enemyAttributes.TakeHit(m_sourceTower);
    }
}
