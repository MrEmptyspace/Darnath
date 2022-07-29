using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using MCEvents;

public class Crop : MonoBehaviour
{
    public CropData cropData;
    public GameObject growingItem;

    public GameObject growthBar;

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
        public float totalGrowthTime;

        public GrowthStage[] growthStages;

        public bool isGrown = false;

        public bool isPlanted = false;

        public int currStageIndex = 0;

        public float currentGrowthTime = 0.0f;

    }



    private void Update()
    {
        //Mostly for UI progress bar
        if (!cropData.isGrown && cropData.isPlanted)
        {
            cropData.currentGrowthTime += Time.deltaTime;
        }

        if (!cropData.isGrown && cropData.isPlanted && cropData.currentGrowthTime > cropData.totalGrowthTime)
        {
            cropData.isGrown = true;
        }


    }

    public Crop DeepCopy()
    {
        GameObject copy = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity);
        Crop sc = copy.GetComponent<Crop>();

        CropData scData = new CropData();
        GrowthStage[] scStages = new GrowthStage[this.cropData.growthStages.Length];
        for (int i = 0; i < this.cropData.growthStages.Length; i++)
        {
            scStages[i] = this.cropData.growthStages[i];
        }

        scData.cropID = this.cropData.cropID;
        scData.cropName = this.cropData.cropName;
        scData.totalGrowthTime = this.cropData.totalGrowthTime;
        scData.isGrown = this.cropData.isGrown;
        scData.currStageIndex = this.cropData.currStageIndex;
        scData.cropID = this.cropData.cropID;

        sc.cropData = scData;
        sc.cropData.growthStages = scStages;

        return sc;
    }


    public void StartGrowingCrop()
    {
        growingItem = transform.gameObject;
        growthBar.transform.position = transform.position + new Vector3(0, 0.25f, 0);

        cropData.isPlanted = true;

        Farm.instance.Grow(cropData.totalGrowthTime, cropData.growthStages.Length, cropData.cropID);
        EventManager.StartListening(MCEventTag.OnStageGrow, OnGrowEvent);
    }

    private void OnGrowEvent(Dictionary<string, object> eventData)
    {
        //This checks to make sure the event matchs with the correct crop
        if ((String)eventData["PlantID"] != cropData.cropID) return;

        // Unsubscribe on the last growth stage
        if (cropData.currStageIndex >= cropData.growthStages.Length)
        {
            CropGrowthCompleted();
        }
        else
        {
            GrowthStage nextStage = cropData.growthStages[cropData.currStageIndex];
            growingItem.GetComponent<Renderer>().material.color = cropData.growthStages[cropData.currStageIndex].visualChange;
            //Debug.Log("PlantID = " + cropData.cropID + " Stage = " + cropData.growthStages[cropData.currStageIndex].description + " Change = " + cropData.growthStages[cropData.currStageIndex].visualChange);
            Debug.Log("Hit growth stage = " + (cropData.currStageIndex) + " currentGrowthTime = " + cropData.currentGrowthTime);
            cropData.currStageIndex++;
        }
    }

    private void CropGrowthCompleted()
    {
        EventManager.TriggerEvent(MCEventTag.GrowthCompleted, EventManager.SingleValue("FinishedCrop", this));
        cropData.isGrown = true;
        EventManager.StopListening(MCEventTag.OnStageGrow, OnGrowEvent);
    }

}
