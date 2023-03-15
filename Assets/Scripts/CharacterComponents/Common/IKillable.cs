using UnityEngine;

public interface IKillable
{
    void TakeDamage(int damage);
    void OnCollisionStay(Collision collision);
    void Destroy();
}
