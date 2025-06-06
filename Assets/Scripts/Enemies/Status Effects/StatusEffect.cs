using UnityEngine;
using System.Collections;

public abstract class StatusEffect : MonoBehaviour
{
    public const float EFFECT_TICK_RATE = 0.25f;

    protected EnemyAttributes enemyAttributes;

    private float m_duration = 0.0f;
    public float elapsedTime { get; protected set; } = 0.0f;

    private void Awake()
    {
        enemyAttributes = GetComponent<EnemyAttributes>();
    }
    public virtual void Init(TowerProperties towerProperties)
    {
        NamedProperty durationProperty = towerProperties.TryGetProperty(NamedProperty.PropertyType.Duration);

        if (durationProperty is null)
        {
            Debug.LogWarning($"Attempting to adjust duration from a tower, {towerProperties.name}, that does not have the duration property.");
            return;
        }

        m_duration = durationProperty.propertyValue;

        StartCoroutine(CheckElapsedTimeRoutine());
    }

    private IEnumerator CheckElapsedTimeRoutine()
    {

        while (elapsedTime < m_duration)
        {
            yield return new WaitForSeconds(EFFECT_TICK_RATE);

            elapsedTime += EFFECT_TICK_RATE;

            PerformEffect();
        }
    }

    public void ExtendDuration(float amount)
    {
        m_duration += amount;
    }
    protected virtual void PerformEffect() { }
}
