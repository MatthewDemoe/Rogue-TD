using UnityEngine;

public class PrefabInstantiator : ProjectileComponent
{
    [SerializeField]
    GameObject prefabToInstantiate;
    
    [SerializeField]
    Transform parent = null;

    [SerializeField]
    bool instantiateInWorldSpace = true;

    public void InstantiatePrefab()
    {
        GameObject instantiatedObject = Instantiate(prefabToInstantiate, parent, instantiateInWorldSpace);

        if (instantiatedObject.TryGetComponent(out ProjectileComponent projectileComponent))
            projectileComponent.Initialize(m_sourceTower);
    }

    public void InstantiateAtParentTransform()
    {
        GameObject instantiatedObject = Instantiate(prefabToInstantiate, parent.position, parent.rotation);

        if (instantiatedObject.TryGetComponent(out ProjectileComponent projectileComponent))
            projectileComponent.Initialize(m_sourceTower);
    }
}
