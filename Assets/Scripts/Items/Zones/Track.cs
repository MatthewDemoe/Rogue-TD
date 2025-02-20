
using Unity.VisualScripting;

public class Track : ItemZone
{
    public override Zone zone => Zone.Track;

    public override bool TryPlacement(HoldableItem item)
    {
        //TODO: if item type is treasure return false
        base.TryPlacement(item);

        if (item.currentZone != zone)
            item.RemoveFromHoldingSlot();

        item.currentZone = zone;
        item.lastPlacement = item.transform.position;
        item.transform.parent = transform;

        return true;
    }
}
