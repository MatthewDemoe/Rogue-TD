using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionButton : MonoBehaviour
{
    TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        GameStateTracker.Instance.OnGameStateChange.AddListener(UpdateButtonState);
        GetComponent<Button>().onClick.AddListener(SetState);
    }

    private void SetState()
    {
        bool atShop = GameStateTracker.Instance.currentState == GameStateTracker.GameState.Shop;

        GameStateTracker.GameState stateToSet = atShop ? GameStateTracker.GameState.Track : GameStateTracker.GameState.Shop;

        bool stateSet = GameStateTracker.Instance.TrySetGameState(stateToSet);
    }

    public void CheckState()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(GameStateTracker.Instance.currentState != GameStateTracker.GameState.Level);
    }

    void UpdateButtonState(GameStateTracker.GameState newState)
    {
        buttonText.text = newState == GameStateTracker.GameState.Shop ? "Track" : "Shop";
        
        CheckState();
    }

    private void OnDestroy()
    {
        GameStateTracker.Instance.OnGameStateChange.RemoveListener(UpdateButtonState);
    }
}
