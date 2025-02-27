using System.Collections.Generic;

public class Wave
{
    private List<WaveEnemyAttributes> m_waveAttributes = new();
    public List<WaveEnemyAttributes> waveAttributes { get { return m_waveAttributes; } }

    private int incomeAmount;
    public Wave() { }
    public Wave(List<WaveEnemyAttributes> waveAttributes, int incomeAmount)
    {
        m_waveAttributes = waveAttributes;
        this.incomeAmount = incomeAmount;   
    }

    public void Start()
    {
        PlayerProperties.Instance.incomeSources.AddIncomeSource(
            new IncomeSource(
                    sourceName: "Wave Income", 
                    amount: () => incomeAmount, 
                    oneTime: true
                ));
    }
}
