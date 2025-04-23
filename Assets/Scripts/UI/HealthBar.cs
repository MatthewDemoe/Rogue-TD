using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Image healthBar;

    [SerializeField]
    Color fullHealthColor = Color.green;

    [SerializeField]
    Color minHealthColor = Color.red;

    private void Start()
    {
        healthBar.color = fullHealthColor;
    }

    public void SetFillAmount(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
        healthBar.color = Color.Lerp(minHealthColor, fullHealthColor, fillAmount);
    }
}
