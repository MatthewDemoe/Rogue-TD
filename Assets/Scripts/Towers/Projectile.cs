using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : DamageSource
{
    [SerializeField]
    float speed = 1.0f;

    private GameObject m_target = null;
    private Vector3 m_direction = Vector3.zero;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (m_target == null)
        {
            Destroy(gameObject);
            return;
        }

        m_direction = (m_target.transform.position - transform.position).normalized;
        rb.velocity = m_direction * speed;
    }

    public override void Initialize(Tower sourceTower)
    {
        base.Initialize(sourceTower);
        m_target = sourceTower.GetTarget().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"Projectile collision");

        other.GetComponent<EnemyAttributes>().TakeHit(m_sourceTower);

        Destroy(gameObject);
    }
}
