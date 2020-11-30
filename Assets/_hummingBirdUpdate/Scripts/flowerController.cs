using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerController : MonoBehaviour
{
    
    Vector3 startScale;
    public Transform lookTarget;
    public Transform[] transformTarget;
    public TargetManager tm;
    public hummingbirdMovement bird;
    public bool cameraFlower;
    
    IEnumerator Start()
    {
        SetupComponents();
        while (!tm)
        {
            if (GameObject.Find("SceneController"))
            {
                tm = GameObject.Find("SceneController").GetComponent<TargetManager>();
            }
            else
            {
                //print("no tm");
                yield return null;
            }
                
        }
        cameraFlower = GetComponentInParent<Camera>();
        SpawnBird();
        startScale = transform.localScale * (UnityEngine.Random.Range(0.8f, 1.2f));
        transform.localScale = Vector3.zero;
        for (float f = 0; f < 1; f+= 2 * Time.deltaTime)
        {
            this.transform.localScale = startScale * f;
            yield return null;
        }
        //tm.sendRandomBirdToSpecificFlower(this);
    }

    private void SetupComponents()
    {
        flowerTarget[] flowers = GetComponentsInChildren<flowerTarget>();
        transformTarget = new Transform[flowers.Length];
        for (int i = 0; i < flowers.Length; i++)
        {
            transformTarget[i] = flowers[i].transform;
        }
    }

    private void SpawnBird()
    {
        if (gameObject.GetComponentInParent<Camera>() == null)
        {
            if (!tm.firstBirdPlaced)
            {
                tm.firstBirdPlaced = true;
                tm.birdButton.SetActive(true);
                bird = tm.birds[0];
            }
            else
            {
                tm.timesFlowerSpawned++;
                bird = GameObject.Instantiate(tm.birdToSpawn[0], Camera.main.transform.position + (Vector3.up * 0.75f),Quaternion.identity).GetComponent<hummingbirdMovement>();
                bird.GetComponent<HumminbirdMaterialManager>().SwitchBirdMaterial(tm.timesFlowerSpawned % 2);
                bird.transform.localScale *= 1.25f;               
            }
            bird.tm = tm;
            bird.targetFlower = this;
            bird.startFlower = this;
            StartCoroutine(delayBirdRoutine());
        }
        tm.UpdateBirdList();
    }

    IEnumerator delayBirdRoutine()
    {
        yield return new WaitForSeconds(1f);
        bird.StartCoroutine("BirdRoutine");
    }

    public IEnumerator KillPlant()
    {
        if (bird)
        {
            print("destroyed");
            bird.flyAway();
        }
        for (float f = 0; f < 1; f += 0.66f * Time.deltaTime)
        {
            transform.localScale = startScale * (1-f);
            yield return null;
        }
        
        Destroy(this.gameObject);

    }
}
