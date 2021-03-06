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

    void Reset()
    {
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movementEnabled = true;
        layerMask = LayerMask.GetMask("Item");
        currentMouseLook.x = -70f;
    }


    void Update()
    {
        if (movementEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            // Get smooth mouse look.
            Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * sensitivity * smoothing);
            appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
            currentMouseLook += appliedMouseDelta;
            currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

            // Rotate camera and controller.
            cam.transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);

            velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            character.Translate(velocity.x, 0, velocity.y);

            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    int numberPressed = i;// + 1;
                    Debug.Log(numberPressed);
                    GameManager.instance.SYS_Player_Inv.HotKeyPressed(numberPressed);
                }
            }

            if (Input.GetKey(KeyCode.E))
            {
                GameManager.instance.SYS_Player_Inv.UseCurrentItem();
            }
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


    void FixedUpdate()
    {
        //If the inventory menu is open
        if (!GameManager.instance.SYS_Player_Inv.gameObject.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.F))
            {
                //Detect if the Player is looking at any item
                PickupItem();
                               
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
 

    private void PickupItem()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, 2.5f, layerMask))
        {
            Transform objectHit = hit.transform;
            currentLookingAt = hit.transform.gameObject;
            //Debug.Log("Looking at = " + objectHit.name);
            if (objectHit.CompareTag("Item"))
            {
                if ((currentLookingAt == null || objectHit.GetComponent<InventoryItem>() != null))
                {
                    InventoryItem itemTmp = objectHit.GetComponent<InventoryItem>();
                    //currentLookingAt = itemTmp;
                    GameManager.instance.SYS_Player_Inv.AddItem(itemTmp);

                }
            }
            else
            {
                currentLookingAt = null;
            }
        }
        else
        {
            currentLookingAt = null;
        }
    }
}
