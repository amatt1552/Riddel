using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]

public class SceneSaver
{
    public int count;
    public List<string> names;
    
    public List<float> positionsX;
    public List<float> positionsY;
    public List<float> positionsZ;

    public List<float> sizesX;
    public List<float> sizesY;
    public List<float> sizesZ;

    public List<float> rotationsX;
    public List<float> rotationsY;
    public List<float> rotationsZ;
    public List<float> rotationsW;

    [NonSerialized]

    private GameObject[] savedObjects;

    public SceneSaver(GameObject[] savedObjects)
    {
        Debug.Log("huhu");
        count = savedObjects.Length;
        this.savedObjects = savedObjects;

        List<float> positionsX = new List<float>();
        List<float> positionsY = new List<float>();
        List<float> positionsZ = new List<float>();

        List<float> sizesX = new List<float>();
        List<float> sizesY = new List<float>();
        List<float> sizesZ = new List<float>();

        List<float> rotationsX = new List<float>();
        List<float> rotationsY = new List<float>();
        List<float> rotationsZ = new List<float>();
        List<float> rotationsW = new List<float>();

        List<string> names = new List<string>();

        for (int i = 0; i < count; i++)
        {
            positionsX.Add(savedObjects[i].transform.position.x);
            positionsY.Add(savedObjects[i].transform.position.y);
            positionsZ.Add(savedObjects[i].transform.position.z);

            rotationsX.Add(savedObjects[i].transform.rotation.x);
            rotationsY.Add(savedObjects[i].transform.rotation.y);
            rotationsZ.Add(savedObjects[i].transform.rotation.z);
            rotationsW.Add(savedObjects[i].transform.rotation.w);

            sizesX.Add(savedObjects[i].transform.localScale.x);
            sizesY.Add(savedObjects[i].transform.localScale.y);
            sizesZ.Add(savedObjects[i].transform.localScale.z);
            names.Add(savedObjects[i].name);
        }

        this.positionsX = positionsX;
        this.positionsY = positionsY;
        this.positionsZ = positionsZ;

        this.rotationsX = rotationsX;
        this.rotationsY = rotationsY;
        this.rotationsZ = rotationsZ;
        this.rotationsW = rotationsW;

        this.sizesX = sizesX;
        this.sizesY = sizesY;
        this.sizesZ = sizesZ;

        this.names = names;
    }

    public SceneSaver()
    {
        GameObject[] savedObjects;
        savedObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        this.savedObjects = savedObjects;
        count = savedObjects.Length;
        
        List<float> positionsX = new List<float>();
        List<float> positionsY = new List<float>();
        List<float> positionsZ = new List<float>();

        List<float> sizesX = new List<float>();
        List<float> sizesY = new List<float>();
        List<float> sizesZ = new List<float>();

        List<float> rotationsX = new List<float>();
        List<float> rotationsY = new List<float>();
        List<float> rotationsZ = new List<float>();
        List<float> rotationsW = new List<float>();

        List<string> names = new List<string>();

        for (int i = 0; i < count; i++)
        {
            positionsX.Add(savedObjects[i].transform.position.x);
            positionsY.Add(savedObjects[i].transform.position.y);
            positionsZ.Add(savedObjects[i].transform.position.z);

            rotationsX.Add(savedObjects[i].transform.rotation.x);
            rotationsY.Add(savedObjects[i].transform.rotation.y);
            rotationsZ.Add(savedObjects[i].transform.rotation.z);
            rotationsW.Add(savedObjects[i].transform.rotation.w);

            sizesX.Add(savedObjects[i].transform.localScale.x);
            sizesY.Add(savedObjects[i].transform.localScale.y);
            sizesZ.Add(savedObjects[i].transform.localScale.z);
            names.Add(savedObjects[i].name);
        }

        this.positionsX = positionsX;
        this.positionsY = positionsY;
        this.positionsZ = positionsZ;

        this.rotationsX = rotationsX;
        this.rotationsY = rotationsY;
        this.rotationsZ = rotationsZ;
        this.rotationsW = rotationsW;

        this.sizesX = sizesX;
        this.sizesY = sizesY;
        this.sizesZ = sizesZ;

        this.names = names;
    }

    public List<Vector3> Positions()
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            positions.Add(new Vector3(positionsX[i], positionsY[i], positionsZ[i]));
        }
        return positions;
    }

    public List<Vector3> Rotations()
    {
        List<Vector3> rotations = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            rotations.Add(new Vector3(rotationsX[i], rotationsY[i], rotationsZ[i]));
        }
        return rotations;
    }

    public List<Vector3> Sizes()
    {
        List<Vector3> sizes = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            sizes.Add(new Vector3(sizesX[i], sizesY[i], sizesZ[i]));
        }
        return sizes;
    }

    public GameObject[] GameObjects()
    {
        return savedObjects;
    }
}

