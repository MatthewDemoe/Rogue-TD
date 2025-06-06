using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SelfDestroyer))]
public abstract class GroundHazard : ProjectileComponent
{
    [SerializeField]
    UnityEvent m_OnDestroyEvent = new();

    public UnityEvent OnDestroyEvent { get { return m_OnDestroyEvent; } }

    SelfDestroyer m_selfDestroyer;

    public override void Initialize(TowerProperties sourceTower)
    {
        base.Initialize(sourceTower);

        NamedProperty durationProperty = m_sourceTower.TryGetProperty(NamedProperty.PropertyType.Duration);

        if (durationProperty is null)
        {
            Debug.LogWarning($"Attempting to get duration from a tower, {m_sourceTower.name}, that does not have the duration property.");
            return;
        }

        m_selfDestroyer = GetComponent<SelfDestroyer>();
        m_selfDestroyer.SetDuration(durationProperty.propertyValue);
    }

    private void OnDestroy()
    {
        m_OnDestroyEvent.Invoke();
    }
}
