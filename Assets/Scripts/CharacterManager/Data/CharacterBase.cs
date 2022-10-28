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
        Debug.Log("CharacterBase awake method - the monobehaviour is in the base class so all inherited members should also have inherit monobehaviour class");
    }

    //Optional Override
    public virtual void Initialise(int speed, Vector3 position, Material mat)
    {
        Name = gameObject.name;
        Speed = speed;
        gameObject.transform.position = position;
        gameObject.GetComponent<Renderer>().material = mat;
    }
}

public class CharacterBase<T> where T : CharacterBase
{
    public GameObject GameObject;
    public T ScriptComponent;
    public CharacterBase(string name)
    {
        //Setting CharacterBase gameObject as a primitive, will replace with a full mesh, model struct
        GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.name = name;

        //setting abitrary script to show how compenents can be added per entity, in this case base will always
        //have his script component of type: newly created class of whatever character created.
        ScriptComponent = GameObject.AddComponent<T>();
    }

}
