using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyActions : MonoBehaviour
{
    private UnityEvent m_OnExited = new();
    public UnityEvent OnExited {get {return m_OnExited;}}

    private UnityEvent m_OnKilled = new();
    public UnityEvent OnKilled {get {return m_OnKilled;}}

    public void Exited()
    {
        OnExited.Invoke();

        Destroy(gameObject);
    }

    public void Killed()
    {
        OnKilled.Invoke();

        Destroy(gameObject);
    }
}
