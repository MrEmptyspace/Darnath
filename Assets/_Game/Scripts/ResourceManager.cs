using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MCEvents;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResourceManager : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI GoldAmount;

    [SerializeField]
    public GameObject SellArea;


    [SerializeField]
    public GameObject BuyDropSpot;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    public List<GameObject> itemLookup = new List<GameObject>();


    public Camera cam;

    int goldAmount;

    //Dictionary<string,TextMeshProUGUI> uiElementsDic;

    // Start is called before the first frame update
    void Start()
    {
        //uiElementsDic = new Dictionary<string, TextMeshProUGUI>();
        //uiElementsDic.Add("gold",GoldText);
        //textmeshPro.SetText("The first number is {0} and the 2nd is {1:2} and the 3rd is {3:0}.", 4, 6.345f, 3.5f);
        EventManager.StartListening(MCEventTag.SellItem, UpdateGold);
        cam = Camera.main;

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        UpdateGold(20);
        BuySeeds();
    }

    private void OnDisable()
    {
        EventManager.StopListening(MCEventTag.SellItem, UpdateGold);
    }

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;

    GameObject previousUI;

    void Update()
    {

        //Set the Pointer Event Position to that of the game object
        m_PointerEventData.position = Input.mousePosition;
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);



        if(results.Count == 0){
            if (previousUI != null)
            {
                previousUI.GetComponent<Image>().color = Color.white;
            }
        }

        for (int i = 0; i < results.Count; i++)
        {

            GameObject uiElement = results[i].gameObject;
            if (previousUI != null)
            {
                if (previousUI != uiElement)
                {
                    previousUI.GetComponent<Image>().color = Color.white;
                }
            }
            uiElement.GetComponent<Image>().color = Color.magenta;
            
            if(Input.GetMouseButtonDown(0)){
                uiElement.GetComponent<Button>().onClick.Invoke();
            }
            // if(uiElement.GetComponent<Image>().color == Color.white){
            //     uiElement.GetComponent<Image>().color = Color.magenta;
            // }else{
            //     uiElement.GetComponent<Image>().color = Color.white;
            // }
            //Debug.Log("Hit " + uiElement.name);
            //  )
            // bool temp = IsPointerOverUIObject(uiElement);
            // Debug.Log(temp);
            // if(){
            // }
            previousUI = uiElement;
        }


        GoldAmount.text = goldAmount + "";

    }

    private bool IsPointerOverUIObject(GameObject ui)
    {
        // get current pointer position and raycast it
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // check if the target is in the UI
        foreach (RaycastResult r in results)
        {
            bool isUIClick = r.gameObject.transform.IsChildOf(ui.transform);
            if (isUIClick)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateGold(int goldAmount)
    {
        this.goldAmount += goldAmount;
    }

    public void SellItems()
    {
        //go through all that is on the table
        SellArea sellArea =  SellArea.GetComponent<SellArea>();
        List<GameObject> sellables = sellArea.sellablesInArea;

        for (int i = 0; i < sellables.Count; i++)
        {
            Debug.Log("Selleable name = " + sellables[i].gameObject.name);
            float amountToAdd = sellables[i].GetComponent<ISellable>().GetBasePrice();
            goldAmount += (int)amountToAdd;
        }

        sellArea.ResetSellArea();
    }

    public void BuySeeds()
    {
        //Get reference to what they are buying.

        int newItemPrice = (int) itemLookup[0].GetComponent<ISellable>().GetBasePrice();
        if(goldAmount > 0){
            if((goldAmount - newItemPrice) > 0 ){
                goldAmount -= newItemPrice;
                //Spawn at location
                Instantiate(itemLookup[0],BuyDropSpot.transform.position,Quaternion.identity);
            }
        }else{
            Debug.Log("NO MONEY");
        }
    }








}
