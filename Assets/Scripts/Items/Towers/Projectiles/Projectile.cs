using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
    TowerProperties m_sourceTower = null;
    
    public void Initialize(TowerProperties sourceTower)
    {
        m_sourceTower = sourceTower;

        GetComponents<ProjectileComponent>().ToList().ForEach(component => component.Initialize(sourceTower));
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        print($"Projectile collision");

        Destroy(gameObject);
    }
}
