using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Camera cam;
    [SerializeField] private KeyCode toggleKey;
    [SerializeField] GameObject objectToToggle;

    Vector2 currentMouseLook;
    Vector2 appliedMouseDelta;
    public float sensitivity = 1;
    public float smoothing = 2;
    public bool movementEnabled;
    public float speed = 5;
    Vector2 velocity;
    GameObject currentLookingAt;
    int layerMask;

    public float pickUpRange = 5;
      [SerializeField] 
    public float moveForce = 250;
    public float throwForce = 5;
    public Transform holdParent;
    public GameObject heldObj;
    public GameObject throwingMarker;
    private GameObject lastHeldObj;

    private GameManager gameManager;


    //
    private bool GetEKey = false;
    private bool GetEKeyDown = false;
    private bool GetMouse0 = false;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;

    Rigidbody rb;

  [SerializeField] 
    public float rbDrag = 5;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movementEnabled = true;
        layerMask = LayerMask.GetMask("Item");
        currentMouseLook.x = -70f;
        rb = character.GetComponent<Rigidbody>();
        rb.freezeRotation = false;
    }

    int ePressCounter = 0;

    float moveSpeed = 100;
    float mouseX;
    float mouseY;
    float yRotation;
    float xRotation;
    float rotationMultiplier;
    void Update()
    {
        GetPlayerInputs();
        ControlDrag();
        if (movementEnabled)
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            character.transform.rotation = Quaternion.Euler(0, yRotation, 0);

            Cursor.lockState = CursorLockMode.Locked;
            // Get smooth mouse look.

            // Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * sensitivity * smoothing);
            // appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
            // currentMouseLook += appliedMouseDelta;
            // currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

            // // Rotate camera and controller.
            // cam.transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
            // character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);

            // velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            // velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            // character.Translate(velocity.x, 0, velocity.y);

            // for (int i = 0; i < keyCodes.Length; i++)
            // {
            //     if (Input.GetKeyDown(keyCodes[i]))
            //     {
            //         int numberPressed = i;// + 1;
            //         Debug.Log(numberPressed);
            //         //GameManager.instance.SYS_Player_Inv.HotKeyPressed(numberPressed);
            //     }
            // }


        }
        else if (!movementEnabled)
        {

        }

        if (Input.GetKeyDown(toggleKey))
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
            movementEnabled = !movementEnabled;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void GetPlayerInputs()
    {
        if (Input.GetKey(KeyCode.E)) //PickingUp
        {
            GetEKey = true;
        }
        if (Input.GetKeyDown(KeyCode.E))//Pressed
        {
            GetEKeyDown = true;

        }
        if (Input.GetMouseButton(0))
        {
            GetMouse0 = true;
        }
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        Vector3 forward = new Vector3(character.transform.forward.x, 0, character.transform.forward.z);
        moveDirection = forward * verticalMovement + character.transform.right * horizontalMovement;

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensitivity * 2;

        xRotation -= mouseY * sensitivity * 2;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    }
    private void FixedUpdate()
    {
        MovePlayer();
        //Move/throw the object because I use force
        if (GetEKey) //PickingUp
        {
            //Debug.DrawRay(character.transform.position, character.transform.TransformDirection(Vector3.forward).normalized * pickUpRange, Color.red, 10f);                //   if (lastHeldObj != currentLookingAt)
            //    {
            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            Debug.DrawRay(ray.origin, ray.direction * pickUpRange, Color.blue, 60f);

            if (Physics.Raycast(ray, out hit, pickUpRange, layerMask))
            {
                Transform objectHit = hit.transform;
                currentLookingAt = hit.transform.gameObject;
                PickupObject(hit.transform.gameObject);
            }
            GetEKey = false;
        }
        if (GetEKeyDown)//Pressed
        {
            if (ePressCounter == 1 && heldObj != null)
            {
                DropObject();
                ePressCounter = 0;
            }
            if (heldObj != null)
            {
                ePressCounter++;
            }
            GetEKeyDown = false;
        }
        if (GetMouse0 && heldObj != null)
        {
            Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
            heldRig.AddForce(throwingMarker.transform.forward * throwForce * 10f);
            DropObject();
        }
        if (heldObj != null)
        {
            MoveObject();
        }
        GetMouse0 = false;
    }
    void DropObject()
    {
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldRig.useGravity = true;
        heldRig.drag = 1;


        lastHeldObj = heldObj;
        heldObj.GetComponent<Collider>().enabled = true;
        heldObj.transform.parent = null;
        heldObj = null;

    }

    void MovePlayer()
    {
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
    }

    void ControlDrag()
    {
        rb.drag = rbDrag;
    }

    void MoveObject()
    {
        //&& velocity.magnitude > 0.0f
        float distance = Vector3.Distance(heldObj.transform.position, holdParent.position);
        Vector3 moveDir = (holdParent.position - heldObj.transform.position);
        Rigidbody heldRig = heldObj.gameObject.GetComponent<Rigidbody>();

        Debug.Log("Distance from heldObject to Hold Parent position" + distance + " | Move Direction = " + moveDir);

        if (distance > 0.1f)
        {
            Debug.DrawRay(heldObj.gameObject.transform.position, moveDir, Color.yellow, 30f);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDir * moveForce);
        }
        else if (distance < 0.099f)
        {
            //heldObj.gameObject.transform.position = holdParent.position;
            heldObj.gameObject.transform.position = Vector3.Lerp(heldRig.position,holdParent.position,0.5f);
        }

        
        Quaternion newRotation = Quaternion.LookRotation(cam.transform.forward);
        heldObj.gameObject.transform.rotation = newRotation;
    }

    void PickupObject(GameObject pickObj)
    {
        Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
        if (objRig)
        {
            holdParent.transform.rotation = Quaternion.identity;
            //pickObj.transform.SetParent(holdParent);
            objRig.position = holdParent.transform.position;
            objRig.useGravity = false;
            objRig.rotation = Quaternion.identity;
            objRig.drag = 10;

            heldObj = pickObj;
            // Rigidbody objRig = pickObj.GetComponent<Rigidbody>();

            
            // objRig.velocity = Vector3.zero;
            // objRig.angularVelocity = Vector3.zero;

            // objRig.transform.parent = holdParent;
            // heldObj = pickObj;

            // heldObj.GetComponent<Collider>().enabled = false;
            // heldObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // heldObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            // heldObj.transform.rotation = Quaternion.identity;
            //heldObj.transform.position = holdParent.position;

        }
    }


    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
         KeyCode.Alpha0,
     };
}
