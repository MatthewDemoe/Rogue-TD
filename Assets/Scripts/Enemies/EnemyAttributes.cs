using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class EnemyAttributes : MonoBehaviour
{
    const float BASE_SPLINE_DURATION = 15.0f;

    [SerializeField]
    private string m_displayName = string.Empty;
    public string displayName { get { return m_displayName; } }

    [SerializeField]
    private int m_minWave = 0;

    public int minWave { get { return m_minWave; } }

    [SerializeField]
    private int m_maxWave = 3;

    public int maxWave { get { return m_maxWave; } }

    [SerializeField]
    private int m_difficultyRating = 1;

    public int difficultyRating { get { return m_difficultyRating; } }

    [SerializeField]
    float baseSpeed = 1.0f;
    float speedMultiplier = 1.0f;
    public float currentSpeed => baseSpeed * speedMultiplier;

    [SerializeField]
    float baseHealth = 1.0f;
    float damageTaken = 0.0f;
    public float currentHealth => baseHealth - damageTaken;

    public float healthPercentage => currentHealth / baseHealth;

    public float distance => splineAnimate.ElapsedTime / splineAnimate.Duration;

    [SerializeField]
    float m_spawnAmountMultiplier = 1.0f;
    public float spawnAmountMultiplier { get { return m_spawnAmountMultiplier; } }

    [SerializeField]
    float m_spawnInterval = 1.0f;
    public float spawnInterval { get { return m_spawnInterval; } }

    [SerializeField]
    SplineAnimate splineAnimate;

    EnemyActions enemyActions;

    [SerializeField]
    private UnityEvent m_OnSpeedChanged = new();

    [SerializeField]
    private UnityEvent<float> m_OnHealthChanged = new();

    public UnityEvent OnSpeedChanged { get { return m_OnSpeedChanged; } }
    public UnityEvent<float> OnHealthChanged { get { return m_OnHealthChanged; } }

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    public SpriteRenderer spriteRenderer => m_spriteRenderer;

    private void Awake()
    {
        if(splineAnimate == null)
            splineAnimate = GetComponent<SplineAnimate>();

        enemyActions = GetComponent<EnemyActions>();

        OnSpeedChanged.AddListener(AdjustAnimationDuration);
        OnSpeedChanged.Invoke();
        OnHealthChanged.AddListener(CheckIfKilled);

        if(m_spriteRenderer is null)
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        damageTaken += amount;
        OnHealthChanged.Invoke(healthPercentage);
    }

    private void CheckIfKilled(float healthPercent)
    {
        if (healthPercent <= 0.0f)
        {
            enemyActions.OnKilled.Invoke();
            Destroy(gameObject);
        }
    }

    private void AdjustAnimationDuration()
    {
        splineAnimate.Duration = BASE_SPLINE_DURATION / currentSpeed;
    }

    public void AddSpeedMultiplier(float newSpeedMultiplier, float duration)
    {
        speedMultiplier *= newSpeedMultiplier;
        OnSpeedChanged.Invoke();
        StartCoroutine(RemoveSpeedMultiplier(newSpeedMultiplier, duration));
    }

    IEnumerator RemoveSpeedMultiplier(float newSpeedMultiplier, float duration)
    {
        yield return new WaitForSeconds(duration);
        speedMultiplier /= newSpeedMultiplier;
        OnSpeedChanged.Invoke();
    }
}
