using UnityEngine;

[RequireComponent(typeof(TowerProperties))]
public abstract class TowerAttacker : MonoBehaviour
{
    public TowerProperties towerProperties { get; private set; } = null;

    protected float timeSinceLastAttack = 0.0f;

    protected abstract void TryAttack();

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
