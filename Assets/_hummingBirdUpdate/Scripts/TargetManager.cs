using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject[] birdToSpawn;
    public float idleTimer;
    public flowerController[] flowersInScene;
    public Transform cameraTarget;
    flowerController currentFlower;
    public hummingbirdMovement[] birds;
    public bool firstBirdPlaced;
    public GameObject birdButton;
    AudioSource AS;
    public int timesFlowerSpawned;

    private void Start()
    {
        birdButton.SetActive(false);
        AS = GetComponent<AudioSource>();
        cameraTarget = Camera.main.gameObject.GetComponentInChildren<flowerController>().transform;
        UpdateBirdList();
        for (int i = 0; i < birds.Length; i++)
        {
            birds[i].tm = this;
            FlyToCamera(birds[i]);
        }
    }

    public void UpdateBirdList()
    {
        birds = GameObject.FindObjectsOfType<hummingbirdMovement>();
    }

    public void newTarget(hummingbirdMovement bird)
    {
        bird.newTarget(selectFlower());
    }

    public void sendRandomBirdToSpecificFlower(flowerController flower)
    {
        hummingbirdMovement bird = birds[Random.Range(0, birds.Length)];
        bird.newTarget(flower);
    }

    public void FlyToCamera(hummingbirdMovement bird)
    {
        bird.newTarget(cameraTarget.GetComponent<flowerController>());
    }

    public void ReturnToStartFlower()
    {
        UpdateBirdList();
        for (int i = 0; i < birds.Length; i++)
        {
            if (birds[i].cameraMode)
            {
                print("camera bird = " + birds[i].gameObject.name);
                birds[i].newTarget(birds[i].startFlower);
            }
        }
        birdButton.SetActive(true);
    }

    public void nearestBirdFlyToCamera()
    {
        GameObject.Find("instructions").GetComponent<instructionManager>().BirdCalled = true;
        birdButton.SetActive(false);
        UpdateBirdList();

        //find nearest bird
        float nearestdistance = 100;
        hummingbirdMovement nearestBird = null;
        for (int i = 0; i < birds.Length; i++)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, birds[i].transform.position);
            if (distance < nearestdistance)
            {
                nearestdistance = distance;
                nearestBird = birds[i];
            }
        }

        if (nearestBird)
            StartCoroutine(delayBirdFly(nearestBird));
    }

    IEnumerator delayBirdFly(hummingbirdMovement bird)
    {
        for (int i = 0; i < 3; i++)
        {
            AS.Play();
            yield return new WaitForSeconds(Random.Range(0.4f, 0.7f));
        }

        FlyToCamera(bird);
        Invoke("ReturnToStartFlower", 10);
    }

    flowerController selectFlower()
    {
        bool flowerSelected = false;
        flowerController flower = flowersInScene[Random.Range(0, flowersInScene.Length)];
        while (!flowerSelected)
        {
            if (flower.bird) {
                print("has bird, try again");
                flowerSelected = true;
            }
            else
                flower = flowersInScene[Random.Range(0, flowersInScene.Length)];
        }
        return flower;
    }

    public void UpdateFlowers()
    {
        int maxFlowers = 5;
        flowersInScene = GameObject.FindObjectsOfType(typeof(flowerController)) as flowerController[];
        if (flowersInScene.Length > maxFlowers)
        {
            if (flowersInScene[maxFlowers].GetComponentInParent<Camera>() != null)
                StartCoroutine(flowersInScene[maxFlowers - 1].KillPlant());
            else            
                StartCoroutine(flowersInScene[maxFlowers].KillPlant());
        }
       
        birds = GameObject.FindObjectsOfType<hummingbirdMovement>();
    }
}
