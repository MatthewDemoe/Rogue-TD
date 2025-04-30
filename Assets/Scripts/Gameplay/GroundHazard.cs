using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SelfDestroyer))]
public abstract class GroundHazard : ProjectileComponent
{
    [SerializeField]
    UnityEvent m_OnDestroyEvent = new();

    public UnityEvent OnDestroyEvent { get { return m_OnDestroyEvent; } }

    SelfDestroyer m_selfDestroyer;

    protected abstract float duration { get; }

    protected virtual void Start()
    {
        m_selfDestroyer = GetComponent<SelfDestroyer>();
        m_selfDestroyer.SetDuration(duration * m_sourceTower.duration);
    }

    private void OnDestroy()
    {
        m_OnDestroyEvent.Invoke();
    }
}
