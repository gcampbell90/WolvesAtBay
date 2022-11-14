using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    [SerializeField] List<AllyScript> myAllies = new List<AllyScript>();
    public List<AllyScript> MyAllies
    {
        get
        {
            return myAllies;
        }
        set
        {
            MyAllies = value;
        }
    }
    public static Transform Commander { get; set; }
    public static GameObject Leader { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        Commander = GameObject.FindGameObjectWithTag("Player").transform;

        Leader =
        //GameObject.CreatePrimitive(PrimitiveType.Cube);
        new GameObject("LeaderGuide");

    }

    private void Update()
    {
        Leader.transform.SetPositionAndRotation(Commander.position, Commander.rotation);
    }
    public static Transform GetNewTransform()
    {
        return Commander;
    }

    public void SetDefensiveFormation()
    {
        foreach (var ally in MyAllies)
        {
            ally.DefensiveFormationCommand();
        }
    }
    public void ChargeCommand()
    {
        foreach (var ally in MyAllies)
        {
            ally.ChargeCommand();
        }
    }
}
