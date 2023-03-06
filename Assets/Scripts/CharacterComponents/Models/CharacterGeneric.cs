using UnityEngine;
/*
 * Generic class for any object inherited from base. Will create entity and attach any components
 * Does this go into another c# file CharacterBaseGeneric?
 */
public class CharacterGeneric<T> where T : CharacterBase
{
    public GameObject GameObject;
    public T ScriptComponent;

    public CharacterGeneric(string name, GameObject CharacterModel)
    {
        //Setting CharacterBase gameObject as a primitive, will replace with a full mesh, model struct
        GameObject = CharacterModel;
        //setting abitrary script to show how compenents can be added per entity, in this case base will always
        //have his script component of type: newly created class of whatever character created.
        ScriptComponent = GameObject.AddComponent<T>();

    }
}


