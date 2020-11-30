using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class hummingbirdMovement : MonoBehaviour
{
    public TargetManager tm;
    Transform birdPositionTarget, birdLookTarget;
    public flowerController 
        targetFlower, 
        startFlower;

    float idleTimer;
    bool dying;
    Animator anim;
    public bool cameraMode;

    public float
        speed = 5,
        lookSpeed = 1;

    int currentFlowerPoint;

    int currentFlower()
    {
        currentFlowerPoint = Random.Range(0, targetFlower.transformTarget.Length);
        return currentFlowerPoint;
    }

    public void newTarget(flowerController flower)
    {
        cameraMode = flower.cameraFlower;

        StopCoroutine("BirdRoutine");

        targetFlower = flower;
        flower.bird = this;

        birdLookTarget.position = flower.lookTarget.position;

        BirdMove(currentFlower());
        StartCoroutine("BirdRoutine");
    }



    void Update()
    {
        CheckSpeed();
        if (tm)
        {
            if (!targetFlower)
            {
            }
            else
            {
                if (!dying)
                    birdLookTarget.position = targetFlower.lookTarget.position;

                heightCheck();
                gameObject.transform.position = birdPositionTarget.position;
                
            }

            Vector3 targetDirection = new Vector3(birdLookTarget.position.x, transform.position.y, birdLookTarget.position.z) - transform.position;
            float singleStep = lookSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    Vector3 previousPosition;

    private void CheckSpeed()
    {
        float speed = (previousPosition - transform.position).magnitude / Time.deltaTime;
        anim.SetFloat("Speed", speed);
        previousPosition = transform.position;
    }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        birdPositionTarget = new GameObject(gameObject.name + "Position").transform;
        birdPositionTarget.transform.position = this.transform.position;
        birdLookTarget =  new GameObject(gameObject.name + "look").transform;
    }

    public void BirdMove(int newFlowerPoint)
    {
        iTween.Stop(gameObject);
        float time;
        float distance = Vector3.Distance(targetFlower.transformTarget[newFlowerPoint].position, transform.position);
        
        //if distance is far away, speed up rotation speed and fly time 
        if (distance > 1)
        {
            lookSpeed = 10;
            time = distance * 0.3f;
        }
        else
        {
            lookSpeed = 4;
            time = 0.25f;
        }

        iTween.MoveTo(birdPositionTarget.gameObject, iTween.Hash("position", targetFlower.transformTarget[newFlowerPoint].position, "time", time, "easeType", iTween.EaseType.easeOutSine));
    }

    public IEnumerator BirdRoutine()
    {
        BirdMove(currentFlower());
        yield return new WaitForSeconds(Random.Range(2, 4));
        StartCoroutine("BirdRoutine");
    }

    void heightCheck()
    {
        if (targetFlower != null)
        {
            RaycastHit raycasthit;
            if (Physics.Raycast(transform.position, (targetFlower.transformTarget[0].position - transform.position), out raycasthit))
            {
                if (raycasthit.transform.tag == "flower" && raycasthit.transform != targetFlower.transform)
                {
                    print("expecting: " + targetFlower.transform.name + " but hit: " + raycasthit.transform.name);
                    gameObject.transform.position += Vector3.up * Time.deltaTime;
                }
            }
        }
    }

    public void flyAway()
    {
        StartCoroutine(FlyAwayRoutine());
    }

    public IEnumerator FlyAwayRoutine()
    {
        iTween.Stop(gameObject);
        StopCoroutine("BirdRoutine");
        dying = true;
        birdLookTarget.transform.position = Camera.main.transform.position + (Vector3.up * 2);
        iTween.MoveTo(birdPositionTarget.gameObject, iTween.Hash("position", birdLookTarget.transform.position, "time", 2f, "easeType", iTween.EaseType.linear));
        iTween.ScaleTo(gameObject, Vector3.zero, 2f);
        yield return new WaitForSeconds(2f);
        Destroy(birdLookTarget.gameObject);
        Destroy(birdPositionTarget.gameObject);
        Destroy(this.gameObject);
    }
}
