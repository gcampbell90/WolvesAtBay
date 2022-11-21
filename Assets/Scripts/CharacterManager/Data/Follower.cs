using UnityEngine;

[System.Serializable]
public sealed class Follower
{
    public GameObject GameObject { get; set; }
    public Vector3 Offset { get; set; }
    public Follower(GameObject follower, Vector3 offset)
    {
        GameObject = follower;
        Offset = offset;
    }

    //public void LerpToVector(Vector3 pos, Quaternion rot)
    //{
    //    GameObject.transform.SetPositionAndRotation(Vector3.Lerp(GameObject.transform.position, pos, Time.deltaTime), Quaternion.Lerp(GameObject.transform.rotation, rot, Time.deltaTime));
    //}
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        GameObject.transform.SetPositionAndRotation(pos, rot);
    }
}
