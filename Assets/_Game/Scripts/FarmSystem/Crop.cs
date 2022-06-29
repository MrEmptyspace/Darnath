using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using MCEvents;

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

    public Crop DeepCopy()
    {
        //Crop sc = new Crop();

        GameObject copy = Instantiate(this.gameObject,Vector3.zero,Quaternion.identity);
        Crop sc = copy.AddComponent<Crop>();

        CropData scData = new CropData();
        GrowthStage[] scStages = new GrowthStage[this.cropData.growthStages.Length];
        for (int i = 0; i < this.cropData.growthStages.Length; i++)
        {
            scStages[i] = this.cropData.growthStages[i];
        }

        scData.cropID = this.cropData.cropID;
        scData.cropName = this.cropData.cropName;
        scData.growthTime = this.cropData.growthTime;
        scData.isGrown = this.cropData.isGrown;
        scData.currStageIndex = this.cropData.currStageIndex;
        scData.cropID = this.cropData.cropID;
        
        sc.cropData = scData;
        sc.cropData.growthStages = scStages;

        return sc;
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
            //EventManager.TriggerEvent(Events.GrowthCompleted, new Dictionary<string, object> { { "PlantID", cropData.cropID } });

            EventManager.TriggerEvent(Events.GrowthCompleted, new Dictionary<string, object> { { "FinishedCrop", this } });
            cropData.isGrown = true;
            EventManager.StopListening(Events.OnStageGrow, OnGrowEvent);
        }
        else
        {
            GrowthStage nextStage = cropData.growthStages[cropData.currStageIndex];
            growingItem.GetComponent<Renderer>().material.color = cropData.growthStages[cropData.currStageIndex].visualChange;
            //Debug.Log("PlantID = " + cropData.cropID + " Stage = " + cropData.growthStages[cropData.currStageIndex].description + " Change = " + cropData.growthStages[cropData.currStageIndex].visualChange);
            cropData.currStageIndex++;

        }
    }
}
