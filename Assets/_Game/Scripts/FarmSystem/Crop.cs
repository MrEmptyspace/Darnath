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
    public bool isActivePlot = true;
    public bool isGrowing = false;
    public int plotNumber;

    public static event Action<CropData,int> OnCropGrowth = delegate { };
    public static event Action<CropData, int> CropCompleted = delegate { };
    //public static event Action<Crop> CropCompletedConversionToItem = delegate { };

    /*public void DestroyGameObject()
    {
        Destroy(growingItem);
    }*/

    public void Init(CropData cropData, int plotNumber)
    {
        this.cropData = cropData;
        this.plotNumber = plotNumber;
    }

    public IEnumerator Grow(int growForSeconds,int untilSeconds)
    {
        isGrowing = true;
        while (growForSeconds != untilSeconds)
        {
            cropData.growthProgress += 1;
            growForSeconds += 1;
            OnCropGrowth(cropData,plotNumber);
            yield return new WaitForSeconds(1);
        }
        StopGrowing();  
    }

    public void StartGrowing()
    {
        //int timeToFinish = cropData.timeToGrow - ;

        if (cropData.growthProgress <= cropData.timeToGrow)
        {
            StartCoroutine(Grow(cropData.growthProgress, cropData.timeToGrow));
        }
        else
        {
            Debug.Log("Crop is already done");
            Debug.Log("Name : " + cropData.cropName + " | Growth : " + cropData.growthProgress);
        }  
    }

    public void StopGrowing()
    {
        CropCompleted(cropData, plotNumber);
        //CropCompletedConversionToItem(this);
        isGrowing = false;
        transform.localScale += new Vector3(4, 4, 4);
        StopCoroutine("Grow");
    }

}
