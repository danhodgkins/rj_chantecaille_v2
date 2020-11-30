using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetPositions : MonoBehaviour
{
    public Transform[] animal;
    animalSelfieLocator[] targetsPositions;
    Transform[] currentTarget = new Transform[2];
    int currentPosition = 1;
    public float speed = 1;
    public bool birdMode;
    private void Start()
    {
        animal[1].GetComponentInChildren<Animator>().SetFloat("offset", 0.5f);
        targetsPositions = GetComponentsInChildren<animalSelfieLocator>();
        if (birdMode)
            nextPosition();
    }

    public void resetPosition()
    {
        for (int i = 0; i < animal.Length; i++)
        {
            animal[i].transform.position = Vector3.zero;
            animal[i].transform.rotation = Quaternion.identity;
        }
        
        GetComponent<touchControllers>().targetScale = Vector3.one;
    }

    private void Update()
    {
        if (birdMode)
        {
            animal[0].transform.position = Vector3.Lerp(animal[0].transform.position, currentTarget[0].position, speed * Time.deltaTime);
            animal[0].transform.rotation = Quaternion.Lerp(animal[0].transform.rotation, currentTarget[0].rotation, speed *  Time.deltaTime);
            animal[1].transform.position = Vector3.Lerp(animal[1].transform.position, currentTarget[1].position, speed * Time.deltaTime);
            animal[1].transform.rotation = Quaternion.Lerp(animal[1].transform.rotation, currentTarget[1].rotation, speed * Time.deltaTime);
        }
    }

    public void nextPosition()
    {
        int TargetPositionValue = currentPosition % targetsPositions.Length - 1;
        print(TargetPositionValue + " " + currentPosition + " " + targetsPositions.Length);
        currentTarget[0] = targetsPositions[TargetPositionValue].transform;
        TargetPositionValue = currentPosition + 1 % targetsPositions.Length - 1;
        currentTarget[1] = targetsPositions[TargetPositionValue].transform;
        //animal.transform.SetPositionAndRotation(targetsPositions[currentPosition].transform.position, targetsPositions[currentPosition].transform.rotation);
        currentPosition++;

        if (currentPosition == targetsPositions.Length)
            currentPosition = 1;
    }
}
