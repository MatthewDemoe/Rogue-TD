using UnityEngine;
public class Poisoned : StatusEffect
{
    public float damage { get; private set; } = 0.0f;
    public override void Init(TowerProperties towerProperties)
    {
        NamedProperty poisonDamage = towerProperties.TryGetProperty(NamedProperty.PropertyType.PoisonDamage);

        if (poisonDamage is null)
        {
            Debug.LogWarning($"Attempting to poison from a tower, {towerProperties.name}, that does not have the poison property.");
            return;
        }

        damage = poisonDamage.propertyValue;

        Init(towerProperties);
    }

    protected override void PerformEffect()
    {
        enemyAttributes.TakeDamage(damage * EFFECT_TICK_RATE);

        base.PerformEffect();
    }
}
