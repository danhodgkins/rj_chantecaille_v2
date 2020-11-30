using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class animalController : MonoBehaviour
{
    float acceleration = 0;
    NavMeshAgent agent;
    Animator anim;

    [Header("animalAttributes")]
    public float accelerationModifier = 1f;
    public float navoffset, navMultiplier;
    public float m_TurningAngleModifier = 1f;
    public float m_SpeedModifier = 1f;
    public float m_ScaleMultiplier = 1f;

    [Header("targets")]

    navTarget targetController;


    int currentTarget;
    Transform[] preferredTargets;

    NavMeshPath mainPath;
    Vector3 lastFacing;
    Vector3 lastPosition;

    float currentAngularVelocity;
    float lerpAngleVel;
    float lerpRotate;
    public float lerpSpeed;

    [SerializeField]
    float distanceToTarget;


    private IEnumerator Start()
    {
        rotateToFaceCamera();
        if (GameObject.Find("NavTarget") != null)
        {
            targetController = GameObject.Find("NavTarget").GetComponent<navTarget>();
            //targetController.transform.localScale = Vector3.one * (0.5f * m_ScaleMultiplier);
            targetController.animal = this;
        }
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        yield return null;
        lerpSpeed = 0;
    }

    public void rotateToFaceCamera()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler((Vector3.up * 30) + Vector3.Scale(Vector3.up, transform.rotation.eulerAngles));
    }

    public void StartWalking()
    {
        CalculatePaths();
    }

    void Update()
    {
        if (targetController != null)
            move();

        AnimationValues();
        
        if (agent)
        {
            agent.speed = this.transform.localScale.x * m_SpeedModifier;
            agent.baseOffset = navoffset + (navMultiplier * (-0.35f / this.transform.localScale.x));
        }

    }

    private void CalculatePaths()
    {
        mainPath = new NavMeshPath();

        NavMesh.CalculatePath(transform.position, targetController.transform.position, NavMesh.AllAreas, mainPath);
        float pathLength = 0;
        for (int i = 0; i < mainPath.corners.Length - 1; i++)
        {
            pathLength += Vector3.Distance(mainPath.corners[i], mainPath.corners[i + 1]);
        }

        currentTarget = 2;
        preferredTargets = targetController.centreTargets;

    }

    private void move()
    {
        {
            distanceToTarget = Vector3.Distance(this.transform.position, targetController.transform.position);
            if (distanceToTarget < 0.65f * m_ScaleMultiplier * (2 * this.transform.localScale.x))
            {
                targetReached();
            }
            if (preferredTargets != null) { 
                Vector3 CornerTargetPosition = preferredTargets[currentTarget].position;
                Vector3 direction = transform.TransformDirection(CornerTargetPosition);
                float distance = Vector3.Distance(CornerTargetPosition, transform.position);


                if ((distance > 0.5f * m_ScaleMultiplier * this.transform.localScale.x) && mainPath != null)
                {
                    acceleration = Mathf.Clamp(acceleration + (accelerationModifier * Time.deltaTime), 0, 1);
                    LookToward(CornerTargetPosition, distance);
                }
                else
                {
                    acceleration = Mathf.Clamp(acceleration - (accelerationModifier * Time.deltaTime), 0, 1);
                }


                Vector3 movement = transform.forward * Time.deltaTime * (transform.localScale.x * m_SpeedModifier * acceleration);
                agent.Move(movement);
            }

        }
    }

    private void LookToward(Vector3 target, float distance)
    {
        var q = Quaternion.LookRotation(target - transform.position);

        float f = Mathf.Abs(transform.rotation.eulerAngles.y - q.eulerAngles.y);
        if (f > 180) f = 360 - f;
        lerpRotate = Mathf.Lerp(lerpRotate, f, Time.deltaTime * 2);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, m_TurningAngleModifier * lerpRotate * Time.deltaTime);
    }

    public void targetReached()
    {
        targetController.hidePin();
        mainPath = null;
        return;
    }

    private void AnimationValues()
    {

        Vector3 currentPosition = transform.position;
        var speed = 100 * Vector3.Magnitude(currentPosition - lastPosition);

        if (hasPath())
            lerpSpeed = Mathf.Lerp(lerpSpeed, speed, Time.deltaTime * 4);
        else
            lerpSpeed = Mathf.Lerp(lerpSpeed, 0, Time.deltaTime * 10);

        Vector3 currentFacing = transform.forward;

        currentAngularVelocity = Vector3.Angle(currentFacing, lastFacing) / Time.deltaTime; //degrees per second

        var cross = Vector3.Cross(currentFacing, lastFacing);
        if (cross.y < 0) currentAngularVelocity = -currentAngularVelocity;

        lerpAngleVel = Mathf.Lerp(lerpAngleVel, currentAngularVelocity, Time.deltaTime * 10);

        lastPosition = currentPosition;
        lastFacing = currentFacing;
        if (anim)
        {

            anim.SetFloat("speed", acceleration);
            //anim.SetFloat("speed", lerpSpeed * (Mathf.Clamp(distanceToTarget / 2,0,1)));
            anim.SetFloat("turnangle", acceleration * lerpAngleVel / 25f);
        }

    }

    public void stopWalking()
    {
        targetReached();
    }

    bool hasPath()
    {
        bool val = false;
        if (mainPath != null)
        {
            val = (mainPath.corners.Length > 0);
        }
        return val;
    }

}