using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ArcProjectileMover : ProjectileMover
{
    [SerializeField]
    float m_duration = 1.0f;

    float m_scaleMultiplier = 2.0f;

    [SerializeField]
    private UnityEvent OnArcEnded = new();

    public override void Initialize(TowerProperties sourceTower)
    {
        base.Initialize(sourceTower);

        StartCoroutine(InterpolateProjectileRoutine());
    }

    private IEnumerator InterpolateProjectileRoutine()
    {
        float elapsedTime = 0.0f;
        float normalizedTime = 0.0f;
        float scaleTime = 0.0f;
        
        float startScale = transform.localScale.x;
        float newScale = 0.0f;

        float newX = 0.0f;
        float newY = 0.0f;
        float newZ = 0.0f;

        Vector3 startPosition = transform.position;
        
        while (elapsedTime <= m_duration)
        {
            normalizedTime = UtilMath.Lmap(elapsedTime, 0.0f, m_duration, 0.0f, 1.0f);

            scaleTime = UtilMath.Lmap(elapsedTime, 0.0f, m_duration, -1.0f, 1.0f);
            scaleTime = 1.0f - Mathf.Abs(scaleTime);

            newScale = UtilMath.EasingFunction.EaseOutSine(startScale, startScale * m_scaleMultiplier, scaleTime);

            newX = UtilMath.EasingFunction.EaseOutSine(startPosition.x, m_target.x, normalizedTime);
            newY = UtilMath.EasingFunction.EaseOutSine(startPosition.y, m_target.y, normalizedTime);
            newZ = UtilMath.EasingFunction.EaseOutSine(startPosition.z, m_target.z, normalizedTime);

            transform.position = new Vector3(newX, newY, newZ);
            transform.localScale = Vector3.one * newScale;

            yield return new WaitForFixedUpdate();
            elapsedTime += Time.fixedDeltaTime;
        }

        OnArcEnded.Invoke();
        Destroy(gameObject);
    }
}
