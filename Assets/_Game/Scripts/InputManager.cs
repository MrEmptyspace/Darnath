using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform character;
    [SerializeField] private Camera cam;
    [SerializeField] GameObject objectToToggle;

    [Header("Player Control")]
    public float moveSpeed = 10;

    [SerializeField]
    float jumpForce = 5f;

    [SerializeField]
    public float heldOjbMoveForce = 250;
    public float throwForce = 5;
    public float additionalGravity = 3f;


    bool isGrounded;

    public float mouseSensitivity = 1;
    public bool movementEnabled;
    GameObject currentLookingAt;
    int itemLayerMask;

    private GameManager gameManager;

    float horizontalMovement;
    float verticalMovement;
    Vector3 moveDirection;
    Rigidbody rb;


    [Header("Held Object")]
    public Transform holdParent;
    public GameObject heldObj;
    public GameObject throwingMarker;
    private GameObject lastHeldObj;
    public float pickUpRange = 5;


    [Header("Keybinds")]
    [SerializeField] KeyCode LeftClick = KeyCode.Mouse0;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode toggleKey;

    private bool GetEKey = false;
    private bool GetEKeyDown = false;
    private bool GetMouse0 = false;

    float mouseX;
    float mouseY;
    float yRotation;
    float xRotation;
    float rotationMultiplier;
    float playerHeight = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movementEnabled = true;
        itemLayerMask = LayerMask.GetMask("Item");
        rb = character.GetComponent<Rigidbody>();
        rb.freezeRotation = false;
    }


    void Update()
    {
        GetPlayerInputs();
        ControlDrag();
        isGrounded = Physics.Raycast(character.position, Vector3.down, playerHeight / 2 + 0.1f);
        Vector3 newPosition = character.position + new Vector3(0, playerHeight / 2 + +0.1f, 0);
        //Debug.DrawRay(newPosition,Vector3.down * (playerHeight / 2),Color.blue,5f);
        //Debug.Log(isGrounded);

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

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

        yRotation += mouseX * mouseSensitivity * 2;

        xRotation -= mouseY * mouseSensitivity * 2;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    }
    private void Jump()
    {
        rb.AddForce(character.up * jumpForce, ForceMode.Impulse);
    }

    float sameItemPickupCD = 0.9f;

    private void FixedUpdate()
    {
        MovePlayer();

        if (GetEKeyDown && heldObj == null) //PickingUp
        {
            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            Debug.DrawRay(ray.origin, ray.direction * pickUpRange, Color.blue, 60f);
            if (Physics.Raycast(ray, out hit, pickUpRange, itemLayerMask) && heldObj == null)
            {
                Transform objectHit = hit.transform;
                currentLookingAt = hit.transform.gameObject;
                PickupObject(hit.transform.gameObject);

            }
            GetEKeyDown = false;
        }
        if (GetEKeyDown && heldObj != null)
        {
            DropObject();
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

        //Start courtine to reset last help object.
        StartCoroutine(WaitAndTrigger(sameItemPickupCD,ResetLastHeldObj));
    }

    private IEnumerator WaitAndTrigger(float waitTime,Action handler)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            handler.Invoke();
        }
    }

    private void ResetLastHeldObj(){
        lastHeldObj = null;
    }

    void MovePlayer()
    {
        //Debug.Log("Move Direction Normalized = " + moveDirection.normalized + " | move speed =" + moveSpeed);
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
        rb.AddForce(Vector3.down * additionalGravity * rb.mass);
    }

    void ControlDrag()
    {
        //rb.drag = rbDrag;
    }

    void MoveObject()
    {
        //&& velocity.magnitude > 0.0f
        float distance = Vector3.Distance(heldObj.transform.position, holdParent.position);
        
        Vector3 moveDir = (holdParent.position - heldObj.transform.position);
        Rigidbody heldRig = heldObj.gameObject.GetComponent<Rigidbody>();
        if (distance > 0.1f)
        {
            Debug.DrawRay(heldObj.gameObject.transform.position, moveDir, Color.yellow, 30f);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDir * heldOjbMoveForce);
        }
        else if (distance < 0.099f)
        {
            //heldObj.gameObject.transform.position = holdParent.position;
            heldObj.gameObject.transform.position = Vector3.Lerp(heldRig.position, holdParent.position, 0.5f);
        }

        Quaternion newRotation = Quaternion.LookRotation(cam.transform.forward);

        Quaternion newRotation2 = Quaternion.LookRotation(holdParent.transform.up);
        heldObj.gameObject.transform.rotation = newRotation2;

    }

    void PickupObject(GameObject pickObj)
    {
        Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
        if (objRig)
        {
            if (pickObj != lastHeldObj)
            {
                holdParent.transform.rotation = Quaternion.identity;
                objRig.position = holdParent.transform.position;
                objRig.useGravity = false;
                objRig.rotation = Quaternion.identity;
                objRig.drag = 10;

                heldObj = pickObj;
            }

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
