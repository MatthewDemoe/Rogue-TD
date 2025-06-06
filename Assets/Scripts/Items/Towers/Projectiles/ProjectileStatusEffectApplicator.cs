using System;
using UnityEngine;

[RequireComponent(typeof(StatusEffect))]
public class ProjectileStatusEffectApplicator : ProjectileComponent
{
    StatusEffect statusEffect;
    private void Start()
    {
        statusEffect = GetComponent<StatusEffect>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyAttributes enemyAttributes))
        {
            enemyAttributes.gameObject.AddComponent(statusEffect.GetType());
            StatusEffect enemyStatusEffect = enemyAttributes.GetComponent<StatusEffect>();
            enemyStatusEffect.Init(m_sourceTower);
        }
    }
}
