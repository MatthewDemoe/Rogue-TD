using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MechanicReferences : MonoBehaviour
{
    public enum MechanicReference
    {
        Damage, 
        Fire, 
        Poison, 
        Slow,
        Range, 
        FireRate
    }

    [SerializeField]
    private List<MechanicReference> m_mechanicReferences = new List<MechanicReference>();

    public List<MechanicReference> mechanicReferences => m_mechanicReferences;

    public List<MechanicReference> ReferenceIntersection(MechanicReferences otherReference)
    {
        List<MechanicReference> intersectingReferences = mechanicReferences.Intersect(otherReference.mechanicReferences).ToList();

        return intersectingReferences;
    }
}
