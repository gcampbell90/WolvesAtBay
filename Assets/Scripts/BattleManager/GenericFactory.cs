using UnityEngine;

public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
{
    protected T _instance;

    public virtual T GetNewInstance()
    {
        return Instantiate(_instance);
    }

}
