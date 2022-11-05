using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugTriggerScript : MonoBehaviour
{
    [SerializeField]
    private UnityEvent LevelTriggeredEvent = new UnityEvent();

    void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.tag == "Player")
        {
            LevelTriggeredEvent.Invoke();
        }
    }
}
