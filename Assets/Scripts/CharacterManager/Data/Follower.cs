using UnityEngine;

[System.Serializable]
public sealed class Follower
{
    public GameObject GameObject { get; set; }
    public Vector3 FormationPosition { get; set; }
    public Follower(GameObject follower, Vector3 offset)
    {
        GameObject = follower;
        FormationPosition = offset;
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
