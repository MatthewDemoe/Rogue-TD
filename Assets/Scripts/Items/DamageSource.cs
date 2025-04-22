using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    protected TowerProperties m_sourceTower = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void Initialize(TowerProperties sourceTower)
    {
        m_sourceTower = sourceTower;
    }
}
