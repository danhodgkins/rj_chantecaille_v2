using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.AI;
using System;
using UnityEngine.EventSystems;

public class ARTapToPlace : MonoBehaviour
{
    Transform cam;
    bool touchEnabled = true;
    public GameObject placementIndicator;
    public chantercailleBoxController makeupbox;
    public GameObject boxShadow;
    private bool boxDropped;
    public GameObject objectToPlace;
    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    GameObject spawnedAnimal;
    public fadeController fade;
    public fadeController uiFade;
    NavMeshAgent agent;
    public instructionManager instructions;
    //public Transform placementIndicator;
    bool safetyMessageAccepted;
    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        cam = Camera.main.transform;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycastManager = arOrigin.gameObject.GetComponent<ARRaycastManager>();
    }

    public void safetyAccepted()
    {
        safetyMessageAccepted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(placeObject());
        }
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && touchEnabled && safetyMessageAccepted)
        {
            if (!boxDropped)
            {
                StartCoroutine(dropBox());
            }
            else
            {
                if (!spawnedAnimal)
                {
                    StartCoroutine(placeObject());
                }
                else
                {
                    walkToLocation();
                }
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        print("touch UI:" + (results.Count > 0));
        return results.Count > 0;
    }

    private IEnumerator dropBox()
    {
        touchEnabled = false;
        instructions.showInstruction(-1);
        makeupbox.Drop();
        boxDropped = true;
        yield return new WaitForSeconds(1f);
        instructions.showInstruction(2);
        touchEnabled = true;
    }

    private void walkToLocation()
    {
        //agent.destination = placementPose.position;
    }

    private IEnumerator placeObject()
    {
        Vector3 spawnLocation = makeupbox.transform.position;
        Quaternion spawnRotation = makeupbox.transform.rotation;
        //placementIndicator.transform.localScale = Vector3.zero;
        instructions.showInstruction(-1);
        touchEnabled = false;
        Quaternion placementPoseRot = placementPose.rotation;
        Vector3 placementPosePos = placementPose.position;
        makeupbox.LidOpen(4);
        GetComponent<AudioManager>().fadeAudio(true);
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(fade.fadeIn(1.5f));
        yield return null;
        
        Destroy(boxShadow);
        
        Destroy(makeupbox.gameObject);
        Instantiate(objectToPlace, spawnLocation, spawnRotation);
        yield return null;
        StartCoroutine(fade.fadeOut(2));
        StartCoroutine(uiFade.fadeIn(2));
        if (ChantecailleARManager.Instance.selectedAnimal != null)
        {
            spawnedAnimal = Instantiate(ChantecailleARManager.Instance.selectedAnimal, spawnLocation, spawnRotation);
            agent = spawnedAnimal.GetComponentInChildren<NavMeshAgent>();
        }
        touchEnabled = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(instructions.showFinalInstructions());
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            Vector3 lookRot = Quaternion.LookRotation(placementIndicator.transform.position,cam.position).eulerAngles;
            lookRot.y += 180;
            lookRot.x = lookRot.z = 0;
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, Quaternion.Euler(lookRot)); //placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            placementIndicator.SetActive(true);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var hits = new List<ARRaycastHit>();
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;  
        if (placementPoseIsValid && !boxDropped && safetyMessageAccepted)
        {
            arOrigin.GetComponent<ARPointCloudManager>().enabled = false;
            instructions.showInstruction(1);
        }
        if (!placementPoseIsValid && !boxDropped && safetyMessageAccepted)
        {
            instructions.showInstruction(0);
        }
        if (placementPoseIsValid)
        {
            
            placementPose = hits[0].pose;            
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.x).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            //placementIndicator.transform.rotation = placementPose.rotation;
        }

    }
}
