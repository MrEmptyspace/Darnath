using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebug : MonoBehaviour
{
    float baseTimeScale;
    // Start is called before the first frame update
    void Start()
    {
        baseTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale == 0.5f)
            {
                Time.timeScale = baseTimeScale;
            }else{
                Time.timeScale = 0.2f;
            }
        }
    }
}
        
