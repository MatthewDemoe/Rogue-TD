using System.Collections.Generic;

public class Wave
{
    private List<WaveAttributes> m_waveAttributes = new();
    public List<WaveAttributes> waveAttributes { get { return m_waveAttributes; } }

    private int incomeAmount;
    public Wave() { }
    public Wave(List<WaveAttributes> waveAttributes, int incomeAmount)
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
