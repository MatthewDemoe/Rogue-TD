using UnityEngine;

public abstract class ProjectileComponent : MonoBehaviour
{
    protected TowerProperties m_sourceTower = null;

    public virtual void Initialize(TowerProperties sourceTower)
    {
        m_sourceTower = sourceTower;
    }
}
