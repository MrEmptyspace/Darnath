using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab;

    [SerializeField]
    private KeyCode newObjectHotkey = KeyCode.A;


    private GameObject currentPlaceableObject;

    private Camera mainCam;

    private float mouseWheelRotation;

    [SerializeField]
    private LayerMask groundMask;

    //Actions
    //Place object

    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleNewObjectHotkey();
        if (currentPlaceableObject != null)
        {
            MoveCurrentPlaceableObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
        }else if(Input.GetMouseButtonDown(1)){
            Destroy(currentPlaceableObject);
        }
    }

    private void RotateFromMouseWheel()
    {
        Debug.Log(Input.mouseScrollDelta.y);
        mouseWheelRotation += (Input.mouseScrollDelta.y * 0.1f);
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation);
    }

    private void HandleNewObjectHotkey()
    {
        if (Input.GetKeyDown(newObjectHotkey))
        {
            currentPlaceableObject = Instantiate(objectPrefab);
        }

    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, groundMask))
        {
            if (hitInfo.collider.name == "Ground")
            {
                //Debug.Log("Raycast hit : " + hitInfo.collider.name);
                currentPlaceableObject.transform.position = hitInfo.point;
                currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }
        }
    }
}
