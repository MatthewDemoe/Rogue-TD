using System.Collections.Generic;
using System.Linq;

public struct IncomeSource
{
    public IncomeSource(string sourceName, IncomeAmount amount, bool oneTime = false)
    {
        this.sourceName = sourceName;
        this.amountCalculation = amount;
        this.oneTime = oneTime;
    }

    public delegate int IncomeAmount();

    public string sourceName;
    public IncomeAmount amountCalculation;
    public bool oneTime;
}

public class PlayerIncomeSources
{
    public List<IncomeSource> incomeSources { get; } = new();

    public int GetTotalIncome()
    {
        return incomeSources.Sum(incomeSource => incomeSource.amountCalculation.Invoke());
    }

    public void AddIncomeSource(IncomeSource incomeSource)
    {
        incomeSources.Add(incomeSource);
    }

    public void RemoveOneTimeIncomeSources()
    {
        incomeSources.RemoveAll(incomeSource => incomeSource.oneTime);
    }

    public void RemoveIncomeSource(IncomeSource incomeSource)
    {
        incomeSources.Remove(incomeSource);
    }
}
