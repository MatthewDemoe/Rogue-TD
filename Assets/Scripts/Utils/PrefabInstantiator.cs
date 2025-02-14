using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
    [SerializeField]
    GameObject prefabToInstantiate;

    
    [SerializeField]
    Transform parent = null;

    [SerializeField]
    bool instantiateInWorldSpace = true;
    
    [SerializeField]
    Vector3 position = Vector3.zero;

    [SerializeField]
    Quaternion rotation = Quaternion.identity;
    

    public void InstantiatePrefab()
    {
        Instantiate(prefabToInstantiate, parent, instantiateInWorldSpace);
    }

    public void InstantiateAtParentTransform()
    {
        Instantiate(prefabToInstantiate, parent.position, parent.rotation);
    }
}
