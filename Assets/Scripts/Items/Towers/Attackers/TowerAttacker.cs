using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TowerProperties))]
public abstract class TowerAttacker : MonoBehaviour
{
    public TowerProperties towerProperties { get; private set; } = null;

    protected float timeSinceLastAttack = 0.0f;

    protected NamedProperty attackRateProperty;

    private void Start()
    {
        towerProperties = GetComponent<TowerProperties>();

        attackRateProperty = towerProperties.TryGetProperty(NamedProperty.PropertyType.AttackRate);

        if (attackRateProperty is null)
        {
            Debug.LogWarning($"Attempting to get attack rate from a tower, {towerProperties.name}, that does not have the attack rate property.");
            return;
        }
    }

    protected virtual void TryAttack()
    {
        timeSinceLastAttack += Time.fixedDeltaTime;

        if (!towerProperties.enemyTracker.enemiesInRange.Any() || (timeSinceLastAttack < attackRateProperty.propertyValue))
            return;

        timeSinceLastAttack = 0.0f;

        Attack();
    }

    protected abstract void Attack();

    private void FixedUpdate()
    {
        TryAttack();
    }
}
