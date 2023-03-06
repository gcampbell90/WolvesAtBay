using UnityEngine;

public interface IKillable
{
    void TakeDamage(int damage);
    void OnCollisionEnter(Collision collision);
    void Destroy();
}
