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
    public string displayName {get {return m_displayName;}}

    [SerializeField]
    float baseSpeed = 1.0f;
    float speedMultiplier = 1.0f;
    public float currentSpeed => baseSpeed * speedMultiplier;

    [SerializeField]
    float baseHealth = 1.0f;
    float damageTaken = 0.0f;
    public float currentHealth => baseHealth - damageTaken;

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

    private UnityEvent OnSpeedChanged = new();
    private UnityEvent OnHealthChanged = new();

    private void Start()
    {
        if(splineAnimate == null)
            splineAnimate = GetComponent<SplineAnimate>();

        enemyActions = GetComponent<EnemyActions>();

        OnSpeedChanged.AddListener(AdjustAnimationDuration);
        OnSpeedChanged.Invoke();
        OnHealthChanged.AddListener(CheckIfKilled);
    }

    public void TakeHit(Tower tower)
    {
        TakeDamage(tower.damage);
    }

    private void TakeDamage(float amount)
    {
        print($"Taking {amount} damage.");

        damageTaken += amount;
        OnHealthChanged.Invoke();
    }

    private void CheckIfKilled()
    {
        if (currentHealth <= 0.0f)
        {
            enemyActions.OnKilled.Invoke();
            Destroy(gameObject);
        }
    }

    private void AdjustAnimationDuration()
    {
        splineAnimate.Duration = BASE_SPLINE_DURATION / currentSpeed;
        Debug.Log($"New Speed : {currentSpeed}");
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
