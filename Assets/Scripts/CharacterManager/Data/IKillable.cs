using UnityEngine;

public interface IKillable
{
    void ITakeDamage(int damage);
    void OnCollisionEnter(Collision collision);

}
