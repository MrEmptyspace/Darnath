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
        public int growthTime;

        public GrowthStage[] growthStages;

        public bool isGrown;

        public int currStageIndex = 0;
    }
    private void Update(){
        //growthBar.value = CalculateGrowthBar();
    }

    public Crop DeepCopy()
    {
        //Crop sc = new Crop();

        GameObject copy = Instantiate(this.gameObject,Vector3.zero,Quaternion.identity);
        Crop sc = copy.GetComponent<Crop>();

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

    public void StartGrowingCrop()
    {
        growingItem = transform.gameObject;
        growthBar.transform.position = transform.position + new Vector3(0,0.25f,0);

        Farm.instance.Grow(cropData.growthTime, cropData.growthStages.Length, cropData.cropID);
        EventManager.StartListening(MCEventTag.OnStageGrow, OnGrowEvent);
    }

    private void OnGrowEvent(Dictionary<string, object> eventData)
    { 
        //This checks to make sure the event matchs with the correct crop
        if ((String)eventData["PlantID"] != cropData.cropID) return;

        // Unsubscribe on the last growth stage
        if (cropData.currStageIndex >= cropData.growthStages.Length)
        {
            //EventManager.TriggerEvent(EventManager.MCEventTag.GrowthCompleted, new Dictionary<string, object> { { "PlantID", cropData.cropID } });

            //EventManager.TriggerEvent(MCEventTag.GrowthCompleted, new Dictionary<string, object> { { "FinishedCrop", this } });
            EventManager.TriggerEvent(MCEventTag.GrowthCompleted, EventManager.SingleValue("FinishedCrop",this));
            cropData.isGrown = true;
            EventManager.StopListening(MCEventTag.OnStageGrow, OnGrowEvent);
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
