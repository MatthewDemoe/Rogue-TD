using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour
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

    [SerializeField]
    float spawnAmountMultiplier = 1.0f;

    [SerializeField]
    float spawnSpeedMultiplier = 1.0f;

    [SerializeField]
    SplineAnimate splineAnimate;

    private UnityEvent OnSpeedChanged = new();

    private void Start()
    {
        if(splineAnimate == null)
            splineAnimate = GetComponent<SplineAnimate>();

        OnSpeedChanged.AddListener(AdjustAnimationDuration);
        OnSpeedChanged.Invoke();
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
