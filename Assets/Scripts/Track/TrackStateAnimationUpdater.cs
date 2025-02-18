using UnityEngine;

public class TrackStateAnimationUpdater : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        GameStateTracker.Instance.OnGameStateChange.AddListener(UpdateAnimationState);
    }

    void UpdateAnimationState(GameStateTracker.GameState newState)
    {
        animator.SetBool("AtShop", newState == GameStateTracker.GameState.Shop);
    }

    private void OnDestroy()
    {
        GameStateTracker.Instance.OnGameStateChange.RemoveListener(UpdateAnimationState);
    }
}
