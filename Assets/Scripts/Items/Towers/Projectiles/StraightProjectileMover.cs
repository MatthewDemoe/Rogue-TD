using UnityEngine;

public class StraightProjectileMover : ProjectileMover
{
    private Vector3 m_direction = Vector3.zero;

    [SerializeField]
    float speed = 1.0f;

    float m_lifetime = 5.0f;

    float m_elapsedTime = 0.0f;

    private void FixedUpdate()
    {
        m_elapsedTime += Time.fixedDeltaTime;

        if (m_elapsedTime >= m_lifetime)
            Destroy(gameObject);
    }

    public override void Initialize(TowerProperties sourceTower)
    {
        base.Initialize(sourceTower);

        m_direction = (m_target - transform.position).normalized;
        rb.linearVelocity = m_direction * speed;
    }
}
