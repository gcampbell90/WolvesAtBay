using UnityEngine;

public interface IKillable
{
    void ITakeDamage(int damage);
    void OnTriggerEnter(Collider collider);
}
