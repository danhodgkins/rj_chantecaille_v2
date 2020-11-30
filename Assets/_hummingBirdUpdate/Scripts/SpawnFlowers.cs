using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlowers : MonoBehaviour
{
    TargetManager tm;
    public GameObject[] flowersToSpawn;
    Camera cam;
    int TotalNumberOfFlowersSpawned;
    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<TargetManager>();
        cam = Camera.main;
    }

    public void NewFlower(Vector3 point)
    {
        GameObject.Find("instructions").GetComponent<instructionManager>().PlantPlaced = true;
        TotalNumberOfFlowersSpawned++;
        GameObject flowerClone = GameObject.Instantiate(flowersToSpawn[TotalNumberOfFlowersSpawned % flowersToSpawn.Length], point, Quaternion.LookRotation(cam.transform.position - point, Vector3.up));
        flowerClone.name = "flowerClone " + TotalNumberOfFlowersSpawned.ToString();
        flowerClone.GetComponent<flowerController>().tm = GetComponent<TargetManager>();
        flowerClone.transform.localRotation = Quaternion.Euler(0, flowerClone.transform.localRotation.eulerAngles.y, 0);
        tm.UpdateFlowers();
    }
}
