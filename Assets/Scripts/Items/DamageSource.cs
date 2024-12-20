using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    protected Tower m_sourceTower = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void Initialize(Tower sourceTower)
    {
        m_sourceTower = sourceTower;
    }
}
