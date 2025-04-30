using UnityEngine;

public class PrefabInstantiator : ProjectileComponent
{
    [SerializeField]
    GameObject prefabToInstantiate;
    
    [SerializeField]
    Transform parent = null;

    [SerializeField]
    bool instantiateInWorldSpace = true;

    private void Start()
    {
        //TODO: Fix this    
        if(prefabToInstantiate.TryGetComponent(out ProjectileComponent projectileComponent))
            projectileComponent.Initialize(m_sourceTower);
    }

    public void InstantiatePrefab()
    {
        Instantiate(prefabToInstantiate, parent, instantiateInWorldSpace);
    }

    public void InstantiateAtParentTransform()
    {
        Instantiate(prefabToInstantiate, parent.position, parent.rotation);
    }
}
