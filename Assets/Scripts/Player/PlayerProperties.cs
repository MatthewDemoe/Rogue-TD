using UnityEngine.Events;

public class PlayerProperties
{
    private static PlayerProperties _instance;

    public static PlayerProperties Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerProperties();

            return _instance;
        }
    }

    public int lives { get; private set; } = 15;
    public int money { get; private set; } = 5;

    public UnityEvent OnLivesChanges { get; private set; } = new();
    public UnityEvent OnMoneyChanged { get; private set; } = new();

    public void AdjustLives(int amount)
    {
        lives += amount;
        OnLivesChanges.Invoke();
    }

    public void AdjustMoney(int amount)
    {
        money += amount;
        OnMoneyChanged.Invoke();
    }
}
