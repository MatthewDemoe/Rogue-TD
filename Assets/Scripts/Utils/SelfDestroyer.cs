using UnityEngine;
using System.Collections;

public class SelfDestroyer : MonoBehaviour
{
    private float m_duration = 0.0f;

    public void SetDuration(float duration)
    {
        m_duration = duration;
        StartCoroutine(DestroyAfterDurationRoutine());
    }

    private IEnumerator DestroyAfterDurationRoutine()
    {        
        float elapsedTime = 0.0f;

        while (elapsedTime < m_duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
