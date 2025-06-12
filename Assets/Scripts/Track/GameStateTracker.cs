using UnityEngine;
using UnityEngine.Events;

public class GameStateTracker
{
    private static GameStateTracker _instance;
    public static GameStateTracker Instance
    {
        get
        { 
            if (_instance == null)
                _instance = new GameStateTracker();

            return _instance;
        } 
    }

    public enum GameState { Shop, Track, Level, Score }
    public GameState currentState { get; private set; } = GameState.Track;

    public UnityEvent<GameState> OnGameStateChange { get; private set; } = new UnityEvent<GameState>();


    public bool TrySetGameState(GameState newState)
    {
        if (newState == currentState)
            return false;

        if (newState == GameState.Level && currentState == GameState.Shop)
            return false;

        currentState = newState;
        OnGameStateChange.Invoke(newState);

        return true;
    }
}
