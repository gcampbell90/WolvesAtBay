using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugTriggerScript : MonoBehaviour
{
    [SerializeField]
    private UnityEvent LevelTriggeredEvent = new UnityEvent();
    private bool collisionOccurred = false;

    void OnTriggerEnter(Collider triggerCollider)
    {
        if (collisionOccurred == false && triggerCollider.tag == "Player")
        {
            collisionOccurred = true;
            LevelTriggeredEvent.Invoke();
        }
        else
        {
            return;
        }
    }
}
