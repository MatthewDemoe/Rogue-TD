using UnityEngine;
using TMPro;

public class WaveContents : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer enemySprite;

    [SerializeField]
    private TextMeshProUGUI enemyCountText;

    public void Initialize(Sprite sprite, int enemyCount)
    {
        enemySprite.sprite = sprite;
        enemyCountText.text = enemyCount.ToString();
    }
}
