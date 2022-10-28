using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Base Character Class - Root of all characters
public abstract class CharacterBase : MonoBehaviour
{
    public string Name { get; set; }
    public int Speed { get; set; }

    //Mandatory Override
    protected abstract void MustBeOverridden();

    void Awake()
    {
        Debug.Log("Character Base awake method");
    }

    //Optional Override
    public virtual void Initialise(int speed, Vector3 position)
    {
        Name = gameObject.name;
        Speed = speed;
        gameObject.transform.position = position;
    }
}

public class CharacterBase<T> where T : CharacterBase
{
    public GameObject GameObject;
    public T ScriptComponent;

    public CharacterBase(string name)
    {
        GameObject = new GameObject(name);
        ScriptComponent = GameObject.AddComponent<T>();
    }
}
