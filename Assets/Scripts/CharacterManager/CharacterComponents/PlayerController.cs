using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    [SerializeField] AllyController allyController;
    private int playerSpeed; //Speed of the player cube
    private int maxVerticalAngleHorizon = 50;
    private int maxHorizontalAngleHorizon = 20;

    //public float mapHeight = 5f; //width of the movable area
    //public float mapWidth = 5f; //width of the movable area
    //public event System.Action OnCubeDeath; //cube death event

    //private Rigidbody rb;

    //float screenHalfWidthInWorldUnits
    public Transform SwordPivot { get; set; }
    public Transform ShieldPivot { get; set; }

    public bool Boost { get; set; }

    public delegate void DefendCommand();
    public static event DefendCommand onDefend;

    public delegate void AttackCommand();
    public static event AttackCommand onAttack;

    private void Awake()
    {
        SwordPivot = gameObject.GetComponentInChildren<Transform>().GetChild(0).transform;
        SwordPivot.rotation = Quaternion.Euler(0, -70, 0);

        ShieldPivot = gameObject.GetComponentInChildren<Transform>().GetChild(1).transform;

    }
    private void Start()
    {
        playerSpeed = gameObject.GetComponent<CharacterBase>().Speed;
    }

    void Update()
    {

        var newrot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.deltaTime * 5);
        HandleInput();

    }
    void HandleInput()
    {

        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
        {

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(BoostBehaviour());
            }

            //Keyboard input and player movement via transform
            Vector3 movementDir = new Vector3(xDir, 0, yDir);
            var newPos = transform.position + new Vector3(0, 0, movementDir.z * GetSpeed() * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * GetSpeed());

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(AttackMove());
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StartCoroutine(DefensiveMove());
            }

        }

        //movementDir.Normalize();

        //old stuff
        //movementDir = movementDir * GetSpeed() * Time.deltaTime;
        //transform.forward += new Vector3(0,0, movementDir.z * GetSpeed() * Time.deltaTime);
        //transform.Translate(GetSpeed() * Time.deltaTime * movementDir, Space.World);

        //Quaternion rotTarget = Quaternion.LookRotation(movementDir, Vector3.up);
        //var newRot = Quaternion.Euler(0, GetClampedValues(Quaternion.LookRotation(movementDir, Vector3.up).eulerAngles.y), 0);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, 500 * Time.deltaTime);

    }

    private float GetClampedValues(float yRotation)
    {
        var yRot = 0;
        if (yRotation == 90)
        {
            yRot = 20;
        }
        else
        if (yRotation == 270)
        {
            yRot = 340;
        }

        return yRot;
    }

    int GetSpeed()
    {
        var boostVal = Boost ? 10 : 5;
        return playerSpeed * boostVal;
    }

    private IEnumerator BoostBehaviour()
    {

        Boost = true;

        //allyController.ChargeCommand();

        while (Input.GetKey(KeyCode.LeftShift))
        {
            yield return null;
        }

        Boost = false;

    }

    float ClampHorizontalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxHorizontalAngleHorizon, maxHorizontalAngleHorizon);
    }

    bool isAttacking = false;
    //TODO: Turn into animation call/method...abstract away from player controller.
    private IEnumerator AttackMove()
    {
        if (!isAttacking)
        {
            isAttacking = true;
        }
        // Just make the animation interval configurable for easier modification later
        float duration = 0.1f;
        float rot = 40;
        float progress = 0f;

        //Play the sword swing audio from the effect controller
        //EffectController.Instance.PlaySwordSound();
        //Debug.Log("PlayerController - attacking");

        onAttack?.Invoke();

        //Get Mouse Position
        //var cam = Camera.main;
        //Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        //// Using some math to calculate the point of intersection between the line going through the camera and the mouse position with the XZ-Plane
        //float t = cam.transform.position.y / (cam.transform.position.y - point.y);
        //Vector3 finalPoint = new Vector3(t * (point.x - cam.transform.position.x) + cam.transform.position.x, 1, t * (point.z - cam.transform.position.z) + cam.transform.position.z);
        //transform.LookAt(finalPoint);

        // Loop until instructed otherwise
        while (progress <= 1f)
        {

            // Do some nice animation
            SwordPivot.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -70, 0), Quaternion.Euler(0, rot, 0), progress);
            progress += Time.deltaTime / duration;

            // Make the coroutine wait for a moment
            // Make the coroutine wait for a momentrdc
            yield return null;
        }

        SwordPivot.localRotation = Quaternion.Euler(0, -70, 0);

        isAttacking = false;
    }

    bool isDefending;
    //TODO: Turn into animation call/method...abstract away from player controller.
    private IEnumerator DefensiveMove()
    {
        isDefending = true;
        onDefend?.Invoke();

        //float rot = 40;
        float timer = 0f;
        float duration = 2f;

        while (!Input.GetKeyUp(KeyCode.Mouse1))
        {
            SwordPivot.gameObject.SetActive(false);
            ShieldPivot.gameObject.SetActive(true);
            yield return null;
        }
        ShieldPivot.gameObject.SetActive(false);
        SwordPivot.gameObject.SetActive(true);
        isDefending = false;
        yield return null;
    }
}



//void OnTriggerEnter(Collider triggerCollider)
//{
//    if (triggerCollider.tag == "Enemy") 
//    {
//        if (OnCubeDeath != null)
//        {
//            OnCubeDeath();
//        }
//        Destroy(gameObject);
//    }
