using UnityEngine;

[System.Serializable]
public class NamedProperty
{
    public enum PropertyType
    {
        ProjectileDamage,
        FireDamage,
        PoisonDamage,
        AttackRate,
        Range,
        Duration,
    }

    [SerializeField]
    private PropertyType m_propertyType;

    public PropertyType propertyType => m_propertyType;

    [SerializeField]
    private float m_propertyValue = 0.0f;

    private float m_propertyBonus = 0.0f;

    public float propertyBonus
    {
        get { return m_propertyBonus; }
        set { m_propertyBonus = value; }
    }

    public float propertyValue => m_propertyValue + propertyBonus;
}
