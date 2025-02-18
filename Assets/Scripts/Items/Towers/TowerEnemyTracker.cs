using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerEnemyTracker : MonoBehaviour
{
    public List<EnemyAttributes> enemiesInRange { get; private set; } = new();

    public UnityEvent<EnemyAttributes> OnEnemyEnteredRange { get; private set; } = new UnityEvent<EnemyAttributes>();
    public UnityEvent<EnemyAttributes> OnEnemyExitedRange { get; private set; } = new UnityEvent<EnemyAttributes>();

    private void OnTriggerEnter(Collider other)
    {
        bool isEnemy = other.gameObject.TryGetComponent(out EnemyAttributes enemy);

        if (!isEnemy)
            return;

        OnEnemyEnteredRange.Invoke(enemy);

        enemiesInRange.Add(enemy);
        enemy.gameObject.GetComponent<EnemyActions>().OnKilled.AddListener(() => enemiesInRange.Remove(enemy));
        enemy.gameObject.GetComponent<EnemyActions>().OnExited.AddListener(() => enemiesInRange.Remove(enemy));
    }

    private void OnTriggerExit(Collider other)
    {
        bool isEnemy = other.gameObject.TryGetComponent(out EnemyAttributes enemy);

        if (!isEnemy)
            return;

        OnEnemyExitedRange.Invoke(enemy);
        enemiesInRange.Remove(enemy);
    }
}
