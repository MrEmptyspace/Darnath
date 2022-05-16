using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData cropData;
    public GameObject growingItem;

    public class GrowthStage
    {
        public string description;
        public Color visualChange;
    }

    [Serializable]
    public class CropData
    {
        public string cropName;

        public string cropID;
        public int growthTime;

        public GrowthStage[] growthStages;

        public bool isGrown;

        public int currStageIndex = 0;


    }



    //public static event Action<Crop> CropCompletedConversionToItem = delegate { };

    /*public void DestroyGameObject()
    {
        Destroy(growingItem);
    }*/

    // public void Init(CropData cropData, int plotNumber)
    // {
    //     this.cropData = cropData;
    //     this.plotNumber = plotNumber;
    // }

    //     public void PlantCrop(CropData cropData, int plotNumber)
    // {
    //     this.cropData = cropData;
    //     this.plotNumber = plotNumber;
    // }

    // public IEnumerator Grow(int growForSeconds,int untilSeconds)
    // {
    //     isGrowing = true;
    //     while (growForSeconds != untilSeconds)
    //     {
    //         cropData.growthProgress += 1;
    //         growForSeconds += 1;
    //         OnCropGrowth(cropData,plotNumber);
    //         yield return new WaitForSeconds(1);
    //     }
    //     StopGrowing();  
    // }

    // public void StartGrowing()
    // {
    //     //int timeToFinish = cropData.timeToGrow - ;

    //     if (cropData.growthProgress <= cropData.timeToGrow)
    //     {
    //         StartCoroutine(Grow(cropData.growthProgress, cropData.timeToGrow));
    //     }
    //     else
    //     {
    //         Debug.Log("Crop is already done");
    //         Debug.Log("Name : " + cropData.cropName + " | Growth : " + cropData.growthProgress);
    //     }  
    // }

    public void StartGrowing()
    {
        growingItem = transform.gameObject;

        Farm.instance.Grow(cropData.growthTime, cropData.growthStages.Length, cropData.cropID);
        EventManager.StartListening(Events.OnStageGrow, OnGrowEvent);
    }

    private void OnGrowEvent(Dictionary<string, object> eventData)
    {
        if ((String)eventData["PlantID"] != cropData.cropID) return;

        // Unsubscribe
        if (cropData.currStageIndex >= cropData.growthStages.Length)
        {
            EventManager.TriggerEvent(Events.GrowthCompleted, new Dictionary<string, object> { { "PlantID", cropData.cropID } });
            cropData.isGrown = true;
            EventManager.StopListening(Events.OnStageGrow, OnGrowEvent);
        }
        else
        {
            GrowthStage nextStage = cropData.growthStages[cropData.currStageIndex];
            growingItem.GetComponent<Renderer>().material.color = cropData.growthStages[cropData.currStageIndex].visualChange;
            Debug.Log("PlantID = " + cropData.cropID + " Stage = " + cropData.growthStages[cropData.currStageIndex].description + " Change = " + cropData.growthStages[cropData.currStageIndex].visualChange);
            cropData.currStageIndex++;
            
        }
    }
}
