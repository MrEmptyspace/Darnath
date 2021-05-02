using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FarmData{
    public string farmName;
    public int farmHeight;
    public int farmWidth;
    public CropData[] crops;
}
