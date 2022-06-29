using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using MCEvents;

public class Farm : MonoBehaviour
{
    //Constructer Init
    public List<Crop> cropsList;

    public Dictionary<string, Crop> cropsLookup = new Dictionary<string, Crop>();
    //public Dictionary<string, Crop.GrowthStage[]> cropsStagesLookup = new Dictionary<string, Crop.GrowthStage[]>();
    GameObject[,] borderedFarmPlots;
    List<GameObject> cropSpots = new List<GameObject>();

    public GameObject carrotPrefab;

    private int plantIDMarker = 0;

    public static Farm instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    string cropIdGen(string cropName)
    {
        plantIDMarker++;
        return cropName + " - " + plantIDMarker + ":" + Guid.NewGuid();
    }

    private void Start()
    {
        EventManager.StartListening(Events.GrowthCompleted, CropCompleted);

        foreach (Transform child in transform)
        {

            cropSpots.Add(child.gameObject);

        }
        //Inline Carrot setup
        Crop.CropData newCropData = new Crop.CropData();
        newCropData.cropName = "Carrot";
        newCropData.cropID = cropIdGen(newCropData.cropName);
        newCropData.growthStages = (new[]
        {
                new Crop.GrowthStage{description = "Stage 0",visualChange = Color.white},
                new Crop.GrowthStage{description = "Stage 1",visualChange = Color.red},
                new Crop.GrowthStage{description = "Stage 2",visualChange = Color.yellow},
                new Crop.GrowthStage{description = "Stage 3",visualChange = Color.green}
            });

        newCropData.growthTime = 1;
        //Crop newCrop = new Crop();
        GameObject tempObj = Instantiate(carrotPrefab, Vector3.zero, Quaternion.identity);
        Crop newCrop = tempObj.AddComponent<Crop>();
        newCrop.cropData = newCropData;
        cropsLookup.Add("carrot", newCrop);
    }
    int cropIndex = 0;

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("This item = " + collider.name);
            Item cropToBeAdd = collider.transform.GetComponent<Item>();
            //string firstFivChar = new string(cropToBeAdd.itemName.Take(5).ToArray());
            if (cropToBeAdd != null)
            {
                if (cropToBeAdd.itemName.StartsWith("Seed-"))
                {
                    //Crop newCropData = CreateNewCropData(cropToBeAdd);
                    string trimmedCropName = cropToBeAdd.itemName.Substring(5).ToLower();
                    Crop newCropData = cropsLookup[trimmedCropName].DeepCopy();
                    newCropData.cropData.cropID = cropIdGen(newCropData.cropData.cropName);
                    newCropData.gameObject.transform.position = cropSpots[cropIndex].transform.position;
                    //ConvertItemToCrop(newCropData);
                    newCropData.StartGrowing();
                    cropIndex++;
                    cropsList.Add(newCropData);

                    //Destroy seeds
                    Destroy(collider.gameObject);
                }
            }
        }

    }

    void ConvertItemToCrop(Crop newCrop)
    {
        //Generate prefab at crop spot positon (TODO make the crop go do the nearest spot)
        GameObject newCropObj = Instantiate(carrotPrefab, cropSpots[0].transform.position, Quaternion.identity);

        Crop newCropTemp = newCropObj.AddComponent<Crop>();
        newCropTemp = newCrop;

        //newCrop.StartGrowing()
        cropsList.Add(newCrop);
    }


    // public Crop CreateNewCropData(Item cropToBeAdd)
    // {
    //     //TODO ADD ERROR HANDLING FOR LOOKUPS
    //     string trimmedCropName = cropToBeAdd.itemName.Substring(5).ToLower();
    //     Crop newCropData = cropsLookup[trimmedCropName].DeepCopy();
    //     //Crop.CropData newCropData = cropsLookup[trimmedCropName].;
    //     //Crop.GrowthStage[] newStages = cropsStagesLookup[trimmedCropName];


    //     return newCropData;
    // }


    public void CropCompleted(Dictionary<string, object> message)
    {
       
        Crop finishedCrop = (Crop)message["FinishedCrop"];

        Debug.Log("Crop Completed Message contents = " + finishedCrop.cropData.cropName + " |  cropID= " + finishedCrop.cropData.cropID);
        
        Rigidbody rb = finishedCrop.growingItem.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(Vector3.up * 10f, ForceMode.Force);

    }


    // Starts a coroutine and returns to the caller after it's time is passed
    public void Grow(int timeToGrow, int stages, string ID)
    {
        StartCoroutine(StartGrowing(timeToGrow, stages, ID));
    }

    private IEnumerator StartGrowing(int timeToGrow, int stages, string ID)
    {
        for (int stage = 0; stage < stages + 1; stage++)
        {
            yield return new WaitForSeconds(timeToGrow);
            //OnStageGrow?.Invoke(ID);
            EventManager.TriggerEvent(Events.OnStageGrow, new Dictionary<string, object> { { "PlantID", ID } });
        }
    }

    //psudeo because drunk
    //Get each crop to call back to the farm to managage it 

    // public void CreateFarmPlots(int farmHeight, int farmWidth)
    // {
    //     int borderSize = 1;
    //     borderedFarmPlots = new GameObject[farmWidth + borderSize, farmHeight + borderSize];

    //     for (int x = 0; x < borderedFarmPlots.GetLength(0); x++)
    //     {
    //         for (int y = 0; y < borderedFarmPlots.GetLength(1); y++)
    //         {
    //             Vector3 spawnPosition = new Vector3(farmPosition.x + x, farmPosition.y - 1, farmPosition.z + y);
    //             Vector3 spacingV = new Vector3(-farmWidth / 2 + .5f + x, 0, -farmWidth / 2 + .5f + y);

    //             if (x == 0 || y == 0 || x == farmWidth + borderSize - 1 || y == farmHeight + borderSize - 1)
    //             {
    //                 GameObject border = Instantiate(borderObject, spawnPosition + spacingV, Quaternion.identity);
    //                 borderedFarmPlots[x, y] = border;
    //             }
    //             else
    //             {
    //                 GameObject empty = Instantiate(emptyPlotObject, spawnPosition + spacingV, Quaternion.identity);
    //                 borderedFarmPlots[x, y] = empty;
    //             }
    //         }
    //     }
    // }

    // public void Init(FarmData farmDataParam, Vector3 farmPosition)
    // {
    //     cropsList = new List<Crop>();
    //     this.farmPosition = farmPosition;
    //     borderObject = Utils.LoadPrefab("borderObject");
    //     emptyPlotObject = Utils.LoadPrefab("emptyPlotObject");
    //     farmData = farmDataParam;

    //     //Setup Farm Plot with border List of GameObjects
    //     CreateFarmPlots(farmData.farmHeight, farmData.farmWidth);

    //     int cropCounter = 0;
    //     for (int x = 0; x < borderedFarmPlots.GetLength(0); x++)
    //     {
    //         for (int y = 0; y < borderedFarmPlots.GetLength(1); y++)
    //         {
    //             Vector3 spawnPosition = new Vector3(farmPosition.x + x, farmPosition.y - 0, farmPosition.z + y);
    //             Vector3 spacingV = new Vector3(-farmData.farmHeight / 2 + .5f + x, 0, -farmData.farmWidth / 2 + .5f + y);

    //             if (borderedFarmPlots[x, y].tag == "Plot")
    //             {
    //                 if (cropCounter < farmData.crops.Length) {

    //                     Destroy(borderedFarmPlots[x,y]);
    //                     var growingItemPrefab = Utils.LoadPrefab(farmData.crops[cropCounter].cropName);
    //                     var growingItemGameObject = Instantiate(growingItemPrefab, spawnPosition + spacingV,Quaternion.Euler(90, 0, 0));

    //                     borderedFarmPlots[x, y] = growingItemGameObject;

    //                     Crop newCrop = borderedFarmPlots[x, y].gameObject.AddComponent<Crop>();
    //                     newCrop.Init(farmData.crops[cropCounter],cropCounter);
    //                     newCrop.growingItem = growingItemGameObject;
    //                     newCrop.StartGrowing();
    //                     cropsList.Add(newCrop);

    //                     cropCounter++;
    //                 }
    //             }
    //         }
    //     }

    //     //Crop handlers
    //     //Crop.CropCompletedConversionToItem += CropConvertToItem;
    // }

    // //BETTER BE A REFERENCE
    // private void CropConvertToItem(Crop crop)
    // {
    //     //Item newItem = new Item(crop.cropData.cropName, crop.cropData.sellValue);
    //     InventoryItem newItem = crop.growingItem.gameObject.AddComponent<InventoryItem>();
    //     newItem.Init(crop.cropData.cropName);
    // }

    // public void PlantNewCrop(string cropName)
    // {

    // }

    // public string CropProgress()
    // {
    //     string cropProgressList = "";

    //     foreach (Crop crop in cropsList)
    //     {
    //         cropProgressList += "\nCrop Name= " + crop.cropData.cropName + " | Crop Progress" + crop.cropData.growthProgress+ "";

    //     }
    //     return cropProgressList;
    // }


    // public override string ToString()
    // {
    //     string result = "Farm\n";
    //     result += string.Format("FarmName: {0}\n", farmData.farmName);
    //     for (int x = 0; x < farmData.farmHeight; x++)
    //     {
    //         result += string.Format("FarmName: {0}\n", farmData.farmName);
    //         result += farmData.crops[x].ToString();
    //     }

    //     return result;
    // }
}
