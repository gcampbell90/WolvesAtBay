using SceneManagerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugTriggerScript : MonoBehaviour
{
    [SerializeField][Range(0,2)] int sceneToLoad;
    [SerializeField]
    private bool collisionOccurred = false;

    void OnTriggerEnter(Collider triggerCollider)
    {
        if (collisionOccurred == false && triggerCollider.tag == "Player")
        {
            collisionOccurred = true;
            GameManager.Instance.LoadScene(sceneToLoad);
        }
        else
        {
            return;
        }
    }
}
