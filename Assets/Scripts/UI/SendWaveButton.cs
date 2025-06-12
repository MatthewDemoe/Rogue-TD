using UnityEngine;
using System.Collections.Generic;

public class SendWaveButton : MonoBehaviour
{
    [SerializeField]
    GameObject waveContentsPrefab;

    List<GameObject> waveContentObjects = new();

    private void Awake()
    {
        GameStateTracker.Instance.OnGameStateChange.AddListener(UpdateButtonState);
    }

    private void Start()
    {
        UpdateWaveContents();
    }

    void UpdateButtonState(GameStateTracker.GameState newState)
    {
        CheckState();
    }

    public void CheckState()
    {
        gameObject.SetActive(GameStateTracker.Instance.currentState == GameStateTracker.GameState.Track);

        //TODO: Change to Score state when implemented
        if (GameStateTracker.Instance.currentState == GameStateTracker.GameState.Track)
        {
            ClearWaveContents();
            UpdateWaveContents();
        }
    }

    private void UpdateWaveContents()
    {
        Dictionary<EnemyAttributes, int> enemyCounts = LevelProperties.Instance.GetEnemyCounts();

        foreach (var item in enemyCounts)
        {
            GameObject waveContentObject = Instantiate(waveContentsPrefab, transform);
            WaveContents waveContents = waveContentObject.GetComponent<WaveContents>();
            waveContentObjects.Add(waveContentObject);

            waveContents.Initialize(item.Key.GetComponentInChildren<SpriteRenderer>().sprite, item.Value);
        }
    }

    private void ClearWaveContents()
    {
        foreach (GameObject content in waveContentObjects)
        {
            Destroy(content);
        }
        waveContentObjects.Clear();
    }

    private void OnDestroy()
    {
        GameStateTracker.Instance.OnGameStateChange.RemoveListener(UpdateButtonState);
    }
}
