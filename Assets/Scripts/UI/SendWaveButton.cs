using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendWaveButton : MonoBehaviour
{
    private void Awake()
    {
        GameStateTracker.Instance.OnGameStateChange.AddListener(UpdateButtonState);
    }

    void UpdateButtonState(GameStateTracker.GameState newState)
    {
        CheckState();
    }

    public void CheckState()
    {
        gameObject.SetActive(GameStateTracker.Instance.currentState == GameStateTracker.GameState.Track);
    }

    private void OnDestroy()
    {
        GameStateTracker.Instance.OnGameStateChange.RemoveListener(UpdateButtonState);
    }
}
