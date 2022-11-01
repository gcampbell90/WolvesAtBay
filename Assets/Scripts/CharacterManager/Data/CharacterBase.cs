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


    /*
     * NOTE: You should use Awake to set up references between scripts, and use Start, which is called after all Awake calls are finished, to pass any information back and forth.
     */

    //void Awake()
    //{
    //    Debug.Log("CharacterBase awake method - the monobehaviour is in the base class so all inherited members should also have inherit monobehaviour class");
    //}

    /*
     * TODO: 
     * Interface? Will allow contract to be created and passed as a ref.
     * Initialising a GO should be neccessary and enforced at code level.
     */
    public virtual void Initialise(int speed, Vector3 position, Material mat)
    {
        Name = gameObject.name;
        Speed = speed;
        gameObject.transform.position = position;
        gameObject.GetComponent<Renderer>().material = mat;
    }

    //This is an example of an method that can be optionally overriden by inherited members.
    public virtual void VirtualMethod(int testInt, string testString)
    {
        Debug.Log("This is an overriden(virtual) method call from the base.");
    }
}

/*
 * Generic class for any object inherited from base. Will create entity and attach any components
 * Does this go into another c# file CharacterBaseGeneric?
 */
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
