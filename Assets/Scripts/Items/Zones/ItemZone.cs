using UnityEngine;

public abstract class ItemZone : MonoBehaviour
{
    public enum Zone { Empty, Shop, Inventory, Track }

    public abstract Zone zone { get; }

    protected Collider[] zoneColliders; 

    protected virtual void Start()
    {
        zoneColliders = GetComponents<Collider>();
        GameStateTracker.Instance.OnGameStateChange.AddListener(CheckActiveInNewState);
        CheckActiveInNewState(GameStateTracker.Instance.currentState);
    }

    public abstract bool TryPlacement(HoldableItem item);

    protected virtual void CheckActiveInNewState(GameStateTracker.GameState newState)
    {

    }
}
