using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MechanicReferences))]
public class Loot : HoldableItem
{
    [SerializeField]
    private float m_bonusAmount = 0.0f;

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

        List<MechanicReferences.MechanicReference> intersectingStats = towerReferences.ReferenceIntersection(lootReferences);
        if(!intersectingStats.Any())
        {
            base.CheckPlacement();
            return;
        }

        intersectingStats.ForEach(stat => collidingTower.BoostStat(stat, m_bonusAmount));

        RemoveFromHoldingSlot();

        Destroy(gameObject);
    }
}
