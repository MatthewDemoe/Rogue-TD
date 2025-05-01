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

    public MechanicReference ReferenceIntersection(MechanicReferences otherReference)
    {
        MechanicReference intersectingReference = mechanicReferences.Intersect(otherReference.mechanicReferences).FirstOrDefault();

        return intersectingReference;
    }
}
