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
        prefabToInstantiate.GetComponent<ProjectileComponent>().Initialize(m_sourceTower);
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
