using UnityEngine;
using System.Collections.Generic;

public class FireArea : GroundHazard
{
    private float damage = 0.01f;

    List<EnemyAttributes> m_enemiesInArea = new();

    private void FixedUpdate()
    {
        m_enemiesInArea.ForEach(enemy => enemy.TakeDamage(damage));
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

        NamedProperty fireDamageProperty = sourceTower.TryGetProperty(NamedProperty.PropertyType.FireDamage);

        if (fireDamageProperty is null)
        {
            Debug.LogWarning($"Attempting to get fire damage from a tower, {sourceTower.name}, that does not have the fire property.");
            return;
        }

        damage *= fireDamageProperty.propertyValue;
    }
}
