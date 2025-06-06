using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Loot : HoldableItem
{
    [SerializeField]
    private float m_bonusAmount = 0.0f;

    [SerializeField]
    NamedProperty.PropertyType lootProperty;
    protected override void CheckPlacement()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        RaycastHit[] allHits = Physics.BoxCastAll(transform.position - Vector3.down, boxCollider.bounds.extents, Vector3.down, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask(new List<string>() { "Item" }.ToArray()));

        bool isCollidingWithTower = allHits.Any();

        if (!isCollidingWithTower)
        {
            base.CheckPlacement();
            return;
        }

        RaycastHit hit = allHits.FirstOrDefault((hit) => hit.collider.TryGetComponent(out TowerProperties tower));
        
        if (hit.collider == null)
        {
            base.CheckPlacement();
            return;
        }

        TowerProperties collidingTower = hit.collider.GetComponent<TowerProperties>();
        NamedProperty towerProperty = collidingTower.TryGetProperty(lootProperty);

        if(towerProperty is null)
        {
            base.CheckPlacement();
            return;
        }

        collidingTower.BoostStat(lootProperty, m_bonusAmount);

        RemoveFromHoldingSlot();

        Destroy(gameObject);
    }
}
