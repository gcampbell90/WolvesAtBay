using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitBox : MonoBehaviour, IHitable
{
    private Collider _collider;
    public Collider Collider { get { return _collider; } private set { _collider = value; } }

    void Awake() 
    {
        _collider = GetComponent<Collider>();
    }

    event EventHandler IHitable.OnColliderHit
    {
        add
        {
            lock (objectLock)
            {
                PostColliderHit += value;
            }
        }

        remove
        {
            lock (objectLock)
            {
                PostColliderHit -= value;
            }
        }
    }


    event EventHandler PostColliderHit;

    object objectLock = new object();

    //public void OnCollisionEnter(Collision collision)
    //{
    //    PostColliderHit?.Invoke(this, EventArgs.Empty);
    //    Debug.Log($"Collision event on {gameObject.name} from {collision.gameObject.name}'s {collision.collider.gameObject.name}");
    //}

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Weapon") return;
        PostColliderHit?.Invoke(this, EventArgs.Empty);
        Debug.Log($"Trigger event on {gameObject.name} from {collision.gameObject.name}");
    }
}

interface IHitable
{
    event EventHandler OnColliderHit;
}

public class MyEventArgs : EventArgs
{
    private Collider _source;

    public MyEventArgs(Collider _source)
    {
        this._source = _source;
    }
}
