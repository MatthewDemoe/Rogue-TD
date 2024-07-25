using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

[RequireComponent(typeof(SphereCollider))]
public class TrackExit : MonoBehaviour
{
    SplineContainer trackSpline;
    SphereCollider exitCollider;

    [SerializeField]
    private UnityEvent<GameObject> m_OnEnemyExited = new();
    public UnityEvent<GameObject> OnEnemyExited { get { return m_OnEnemyExited; } }

    void Start()
    {
        trackSpline = GetComponentInParent<SplineContainer>();
        exitCollider = GetComponent<SphereCollider>();

        List<BezierKnot> knots = trackSpline.Spline.Knots.ToList();
        exitCollider.center = knots[knots.Count - 1].Position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision with {other.name}");

        if (!other.TryGetComponent(out EnemyActions enemyActions))
            return;

        enemyActions.Exited();
        OnEnemyExited.Invoke(other.gameObject);
    }
}
