using UnityEngine;
using System.Collections.Generic;

public class FireArea : GroundHazard
{
    private float damage = 0.01f;

    protected override float duration => 2.5f;

    List<EnemyAttributes> m_enemiesInArea = new();

    private void FixedUpdate()
    {
        m_enemiesInArea.ForEach(enemy => enemy.TakeHit(damage));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyAttributes enemyAttributes))
        {
            m_enemiesInArea.Add(enemyAttributes);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyAttributes enemyAttributes))
        {
            m_enemiesInArea.Remove(enemyAttributes);
        }
    }

    public override void Initialize(TowerProperties sourceTower)
    {
        base.Initialize(sourceTower);
        damage *= sourceTower.damage;
    }
}
