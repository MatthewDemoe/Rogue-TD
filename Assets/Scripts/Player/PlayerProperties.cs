using UnityEngine;
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

    public PlayerProperties()
    {
        incomeSources.AddIncomeSource(new IncomeSource
            (
                sourceName: "Interest",
                amount: () => { return interest; },
                oneTime: false
            ));

        Level.Instance.OnWaveCompleted.AddListener(() => 
        {
            foreach (IncomeSource source in incomeSources.incomeSources)
            {
                Debug.Log($"{source.sourceName} : {source.amountCalculation.Invoke()}");
            }

            Debug.Log($"Total Income: {incomeSources.GetTotalIncome()}");

            AdjustMoney(incomeSources.GetTotalIncome());
            incomeSources.RemoveOneTimeIncomeSources();
        });
    }

    public int lives { get; private set; } = 15;
    public int money { get; private set; } = 5;

    public int interest => money / 5;

    public bool isHoldingTower { get; set; } = false;
    public GameObject heldTower = null;

    public UnityEvent OnLivesChanges { get; private set; } = new();
    public UnityEvent OnMoneyChanged { get; private set; } = new();

    public PlayerIncomeSources incomeSources { get; } = new();

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
