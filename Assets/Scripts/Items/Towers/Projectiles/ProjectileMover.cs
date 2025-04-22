using UnityEngine;

public abstract class ProjectileMover : ProjectileComponent
{
    protected Vector3 m_target = Vector3.zero;

    protected Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Initialize(TowerProperties sourceTower)
    {
        base.Initialize(sourceTower);

        m_target = sourceTower.enemyTracker.GetTarget().gameObject.transform.position;
    }
}
