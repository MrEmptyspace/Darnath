using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MCEvents;

public class ResourceManager : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI GoldAmount;

    [SerializeField]
    GameObject SellArea;


    int goldAmount;

    //Dictionary<string,TextMeshProUGUI> uiElementsDic;

    // Start is called before the first frame update
    void Start()
    {


        //uiElementsDic = new Dictionary<string, TextMeshProUGUI>();
        //uiElementsDic.Add("gold",GoldText);
        //textmeshPro.SetText("The first number is {0} and the 2nd is {1:2} and the 3rd is {3:0}.", 4, 6.345f, 3.5f);
        EventManager.StartListening(MCEventTag.DebugEvent, UpdateGold);
    }

    private void OnDisable()
    {
        EventManager.StopListening(MCEventTag.DebugEvent, UpdateGold);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            EventManager.TriggerEvent(MCEventTag.DebugEvent, 5);
        }
        GoldAmount.text = goldAmount+"";
    }


    public void UpdateGold(int goldAmount)
    {
        this.goldAmount += goldAmount;
    }

}
