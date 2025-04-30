using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TowerProperties))]
public abstract class TowerAttacker : MonoBehaviour
{
    public TowerProperties towerProperties { get; private set; } = null;

    protected float timeSinceLastAttack = 0.0f;

    protected virtual void TryAttack()
    {
        timeSinceLastAttack += Time.fixedDeltaTime;

        if (!towerProperties.enemyTracker.enemiesInRange.Any() || (timeSinceLastAttack < towerProperties.fireRate))
            return;

        timeSinceLastAttack = 0.0f;

        Attack();
    }

    protected abstract void Attack();

    private void Start()
    {
        towerProperties = GetComponent<TowerProperties>();
    }

    private void FixedUpdate()
    {
        TryAttack();
    }
}
