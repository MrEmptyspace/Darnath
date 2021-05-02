using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class Utils
{

    public static GameObject LoadPrefab(string assetName)
    {
        GameObject returnPrefab = new GameObject();
        try
        {
            returnPrefab = AssetDatabase.LoadAssetAtPath("Assets/Resources/" + assetName + ".prefab", typeof(GameObject)) as GameObject;
        }
        catch (Exception e)
        {
            Debug.Log("Could not load Asset named :" + assetName);
        }
        return returnPrefab;
    }


    public static Sprite LoadTexture(string assetName, string fileType, string subFolderInResources = "")
    {
        Sprite returnPrefab = null;// = new Texture();
        string assetType = "Sprite";
        try
        {
            string filePath = "Assets/Resources/" + subFolderInResources + assetName + "." + fileType + "";
            returnPrefab = AssetDatabase.LoadAssetAtPath(filePath, typeof(Sprite)) as Sprite;
        }
        catch (Exception ex)
        {
            Debug.Log("Could not load Texture named :" + assetName + " At " + "Assets/Resources/" + subFolderInResources + assetName + "." + fileType + "" + "As a " + assetType);
            throw;
        }
        return returnPrefab;
    }



    public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }

    public static String ReadJsonFile(string path)
    {
        string returnVal;
        try
        {
            using (StreamReader stream = new StreamReader(path))
            {
                returnVal = stream.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Could not read file");
            Debug.Log(e);
            throw;
        }
        return returnVal;
    }

    public static void WriteJsonFile(string path, string fileName, string farmDataArrayJson)
    {
        System.IO.File.WriteAllText(path + fileName, farmDataArrayJson);
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        Debug.Log("Could not find a parent with given tag." + tag);
        return null; // Could not find a parent with given tag.
    }
}
