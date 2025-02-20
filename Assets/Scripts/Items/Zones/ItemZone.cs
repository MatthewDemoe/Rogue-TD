using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemZone : MonoBehaviour
{
    public enum Zone { Empty, Shop, Inventory, Track }

    public abstract Zone zone { get; }

    public virtual bool TryPlacement(HoldableItem item)
    {
        item.transform.parent = null;

        return true;
    }
}
