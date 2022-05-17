using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Farm : MonoBehaviour
{
    //Constructer Init
    public List<Crop> cropsList;
    GameObject[,] borderedFarmPlots;
    //public FarmData farmData;
    //public Vector3 farmPosition;

    //public GameObject borderObject;
    //public GameObject emptyPlotObject;

    //public event Action OnStageGrow;

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

    private void Start()
    {
        EventManager.StartListening(Events.GrowthCompleted, CropCompleted);

        //Find all the crops and init them
        cropsList = new List<Crop>(FindObjectsOfType<Crop>());

        // foreach (Crop crop in cropsList)
        // {
        //     crop.cropData = CreateNewCropData();
        //     crop.StartGrowing();
        // }

        // cropsList[0].cropData = CreateNewCropData();
        // cropsList[0].StartGrowing();

        // cropsList[1].cropData = CreateNewCropData();
        // cropsList[1].StartGrowing();

        // cropsList[2].cropData = CreateNewCropData();
        // cropsList[2].StartGrowing();
    }

    public Crop.CropData CreateNewCropData()
    {
        Crop.CropData newCrop = new Crop.CropData();
        newCrop.cropName = "Carrot";
        newCrop.cropID = newCrop.cropName + " - " + plantIDMarker + ":" + Guid.NewGuid();
        newCrop.growthStages = (new[]
        {
        new Crop.GrowthStage{description = "Stage 0",visualChange = Color.white},
            new Crop.GrowthStage{description = "Stage 1",visualChange = Color.red},
            new Crop.GrowthStage{description = "Stage 2",visualChange = Color.yellow},
            new Crop.GrowthStage{description = "Stage 3",visualChange = Color.green}
        });

        newCrop.growthTime = 5;
        plantIDMarker++;
        return newCrop;
    }

    public void CropCompleted(Dictionary<string, object> message)
    {
        Debug.Log("Crop Completed Message contents = " + message.ToString());
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
