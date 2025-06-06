using UnityEngine;

public class ProjectileDamageDealer : ProjectileComponent
{

    private NamedProperty m_projectileDamageProperty;
    private void Start()
    {
        m_projectileDamageProperty = m_sourceTower.TryGetProperty(NamedProperty.PropertyType.ProjectileDamage);

        if (m_projectileDamageProperty is null)
        {
            Debug.LogWarning($"Attempting to get projectile damage from a tower, {m_sourceTower.name}, that does not have the projectile damage property.");
            return;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyAttributes enemyAttributes))
            enemyAttributes.TakeDamage(m_projectileDamageProperty.propertyValue);
    }
}
