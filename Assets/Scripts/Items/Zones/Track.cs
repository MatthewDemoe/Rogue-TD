
using Unity.VisualScripting;
using UnityEngine;

public class Track : ItemZone
{
    public override Zone zone => Zone.Track;

    public override bool TryPlacement(HoldableItem item)
    {
        //TODO: if item type is treasure return false
        if(item is Loot)
            return false;

        if (item.currentZone != zone)
            item.RemoveFromHoldingSlot();

        item.SetZone(this);

        return true;
    }
    protected override void CheckActiveInNewState(GameStateTracker.GameState newState)
    {
        base.CheckActiveInNewState(newState);

        bool shouldBeActive = newState != GameStateTracker.GameState.Shop;

        foreach (Collider collider in zoneColliders)
        {
            collider.enabled = shouldBeActive;
        }
    }
}
