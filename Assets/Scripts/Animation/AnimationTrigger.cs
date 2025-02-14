using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField]
    UnityEvent eventToTrigger = new UnityEvent();

    public void TriggerEvent()
    {
        eventToTrigger.Invoke();
    }
}
