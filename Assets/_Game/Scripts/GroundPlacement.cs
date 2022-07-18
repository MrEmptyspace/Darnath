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
    public LayerMask groundMask;

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
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentPlaceableObject);
        }
    }


    private void RotateFromMouseWheel()
    {
        //TODO Turn these into variables for rotating object
        mouseWheelRotation += Input.mouseScrollDelta.y * 15;
        mouseWheelRotation = Mathf.Lerp(mouseWheelRotation, 0, Time.deltaTime *5);

        Quaternion lookAtRotation = currentPlaceableObject.transform.rotation;

        //Hard code offset fix
        lookAtRotation *= Quaternion.Euler(0, 0, mouseWheelRotation);
        currentPlaceableObject.transform.rotation = Quaternion.Slerp(currentPlaceableObject.transform.rotation, lookAtRotation, Time.deltaTime * 1.0f);

    }

    private void HandleNewObjectHotkey()
    {
        if (Input.GetKeyDown(newObjectHotkey))
        {
            currentPlaceableObject = Instantiate(objectPrefab);
            //currentPlaceableObject.transform.Rotate(-90f, 0f, 0f);
            Transform[] temp = currentPlaceableObject.GetComponentsInChildren<Transform>();
            List<Transform> snapPointsList = new List<Transform>(temp);
            snapPointsList = snapPointsList.FindAll(gb => gb.tag == "SnapSpot");
            //List<GameObject> snapPointsList = new List<GameObject>(currentPlaceableObject.GetComponentsInChildren<GameObject>()).FindAll(gb => gb.tag =="SnapSpot");
            snapPointsList.ForEach(point => Debug.Log(point.transform.position));
            mouseWheelRotation = 0;
        }
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        RaycastHit[] hits;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Debug.Log("Raycast hit : " + hit.collider.name);
            if (hit.collider.name == "Ground")
            {
                Debug.Log("Raycast hit : " + hit.collider.name);
                currentPlaceableObject.transform.position = hit.point;
                //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }
        }

        // Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        // RaycastHit hitInfo;
        // if (Physics.Raycast(ray, out hitInfo, groundMask))
        // {
        //     Debug.Log("Raycast hit : " + hitInfo.collider.name);
        //     if (hitInfo.collider.name == "Ground")
        //     {
        //         Debug.Log("Raycast hit : " + hitInfo.collider.name);
        //         currentPlaceableObject.transform.position = hitInfo.point;
        //         //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        //     }
        // }
    }
}
