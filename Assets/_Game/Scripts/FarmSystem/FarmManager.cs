using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class FarmManager : MonoBehaviour
{
    //public List<Farm> FarmListGlobal = new List<Farm>();

    [SerializeField]
    public List<GameObject> FarmGameObjects = new List<GameObject>();

     List<Farm> farmScripts = new List<Farm>();

    public GameObject testFarmPosition;

    string farmDataClassOutput = "Assets/_Game/FarmData/testFarmJson.json";
    //string farmDataClassOutput = "Assets/_Game/FarmData/TestOutputClass.json";

    private void Start()
    {
        SetupFarms();
    }

    void SetupFarms()
    {
        //WriteJsonFile(outputFarmDataPath, "TestOutputClass.json", JsonUtility.ToJson(test));

        FarmGameObjects = Utils.FindObjectsWithTag(transform,"Farm");

        string farmsJsonString = Utils.ReadJsonFile(farmDataClassOutput);
        JSONNode farmArrayJson = JSON.Parse(farmsJsonString);

        int farmsAssigned = 0;
    
        foreach (JSONNode farmData in farmArrayJson["farms"])
        {
            //Need to have a list of gameobjects for the farms to be attached to
            if(farmsAssigned <= FarmGameObjects.Count && FarmGameObjects.Count != 0) 
            {
                FarmData newFarmData = JsonUtility.FromJson<FarmData>(farmData.ToString());
                Farm newfarm = FarmGameObjects[farmsAssigned].AddComponent<Farm>();
                farmScripts.Add(newfarm);
                //THIS SHOULD BE READ FROM THE START POSITION OF THE FARM GAME OBJECT LIST
                newfarm.Init(newFarmData,testFarmPosition.transform.position);
                //Farm newFarm = new Farm(newFarmData, testFarmPosition.transform.position);
                //FarmListGlobal.Add(newFarm);
            }
            else
            {
                Debug.Log("Farm data does not match Game Object farms, Likely add more farms");
            }
        }
    }

    

    public void DeleteFarms()
    {
        /*foreach (Farm farm in FarmListGlobal)
        {
            //farm.DeleteFarm();
        }*/
    }

    public void RecreateFarm()
    {
        /*foreach (Farm farm in FarmListGlobal)
        {
            farm.RecreateFarm();
        }*/
    }

    public string FarmDisplay()
    {
        string farmProgressList = "";
        foreach (Farm farm in farmScripts)
        {
            farmProgressList += "Farm Name: " + farm.farmData.farmName;
            farmProgressList +=farm.CropProgress();
        }
        return farmProgressList;
    }
   

}
