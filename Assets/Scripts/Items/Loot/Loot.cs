using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MechanicReferences))]
public class Loot : HoldableItem
{
    protected override void CheckPlacement()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        RaycastHit[] allHits = Physics.BoxCastAll(transform.position - Vector3.down, boxCollider.bounds.extents, Vector3.down, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask(new List<string>() { "Tower" }.ToArray()));

        bool isCollidingWithTower = allHits.Any();

        if (!isCollidingWithTower)
        {
            base.CheckPlacement();
            return;
        }

        TowerProperties collidingTower = allHits.FirstOrDefault((hit) => hit.collider.TryGetComponent(out TowerProperties tower)).collider.GetComponent<TowerProperties>();

        MechanicReferences lootReferences = GetComponent<MechanicReferences>();
        MechanicReferences towerReferences = collidingTower.GetComponent<MechanicReferences>(); 

        MechanicReferences.MechanicReference statToBoost = towerReferences.ReferenceIntersection(lootReferences);

        collidingTower.BoostStat(statToBoost);

        RemoveFromHoldingSlot();

        Destroy(gameObject);
    }
}
