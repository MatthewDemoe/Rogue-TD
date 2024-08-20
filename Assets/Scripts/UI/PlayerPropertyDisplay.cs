using TMPro;
using UnityEngine;

public class PlayerPropertyDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI livesText;

    [SerializeField]
    TextMeshProUGUI moneyText;

    void Start()
    {
        PlayerProperties.Instance.OnMoneyChanged.AddListener(UpdateMoneyUI);
        PlayerProperties.Instance.OnLivesChanges.AddListener(UpdateLivesUI);
    }

    private void UpdateLivesUI()
    {
        livesText.text = $"<3 : {PlayerProperties.Instance.lives}";
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = $"$ : {PlayerProperties.Instance.money}";
    }
}
