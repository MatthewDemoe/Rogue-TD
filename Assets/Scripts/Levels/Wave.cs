using System.Collections.Generic;

public class Wave
{
    private List<WaveAttributes> m_waveAttributes = new();
    public List<WaveAttributes> waveAttributes { get { return m_waveAttributes; } }

    public Wave() { }
    public Wave(List<WaveAttributes> waveAttributes)
    {
        m_waveAttributes = waveAttributes;
    }
}
