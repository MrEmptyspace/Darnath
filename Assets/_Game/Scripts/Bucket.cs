using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.layer = LayerMask.NameToLayer("InBucketLayer");
        if (TryGetComponent<Rigidbody>(out var rb))
        {
           // rb.AddForce(Vector3.up, ForceMode.Force);
        }
        //collider.gameObject.GetComponent<Rigidbody>()
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InBucketLayer"))
        {
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                //rb.AddForce(-(Physics.gravity * 0.99f));
            }
        }

    }

    void OnTriggerExit(Collider collider)
    {
        collider.gameObject.layer = LayerMask.NameToLayer("Item");
    }
}
