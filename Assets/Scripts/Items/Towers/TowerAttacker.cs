using UnityEngine;

[RequireComponent(typeof(TowerProperties))]
public abstract class TowerAttacker : MonoBehaviour
{
    public TowerProperties towerProperties { get; private set; } = null;

    protected float timeSinceLastAttack = 0.0f;

    protected abstract void TryAttack();

    private void Start()
    {
        towerProperties = GetComponent<TowerProperties>();
    }

    private void FixedUpdate()
    {
        TryAttack();
    }
}
