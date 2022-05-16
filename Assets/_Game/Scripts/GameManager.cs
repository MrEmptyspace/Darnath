using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else

        {
            instance = this;
        }


    }


    

    public InputManager SYS_Input;
    public Inventory SYS_Player_Inv;

    //etc.

}
