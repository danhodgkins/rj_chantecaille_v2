using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class navTarget : MonoBehaviour
{
    public GameObject pin;
    public LayerMask layermask;
    Transform cam;
    public animalController animal;
    Vector3 offset;
    public Transform[] 
        rightTargets, 
        leftTargets, 
        centreTargets;

    public float holdTime;
    public bool dragging;
    Vector3 startDragPos, currentDragPos;
    float touchDelay = 0;

    public RecordingManager recorder;

    void Start()
    {
        hidePin();
        cam = Camera.main.transform;
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(cam.position.x,this.transform.position.y,cam.position.z));
        
        transform.localRotation *= Quaternion.Euler(Vector3.up * 180);
        touchDelay -= Time.deltaTime;
        if (touchDelay < 0) touchDelay = 0;
        if (Input.touchCount > 1)
            touchDelay = 0.5f;
        if (touchDelay < 0.05f)
        {
            if (Input.GetMouseButtonDown(0) && Input.touchCount < 2)
            {
                if (!IsPointerOverUIObject())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.transform.gameObject.tag == "Animal")
                        {
                            animal.stopWalking();
                            startDragPos = hit.point;
                            startDragPos.y = animal.transform.position.y;
                            currentDragPos = startDragPos;
                            currentDragPos.y = startDragPos.y;
                            dragging = true;
                            offset = Vector3.zero;
                            if (Physics.Raycast(ray, out RaycastHit hit2, 100, layermask))
                            {
                                offset = animal.transform.position - hit2.point;
                                print(offset);
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (!IsPointerOverUIObject())
                {
                    holdTime += Time.deltaTime;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit,100, layermask))
                    {
                        if (!dragging)
                        {
                            if (Vector3.Distance(hit.point, animal.transform.position) > 0.5f)
                            {
                                
                                this.transform.position = hit.point;
                                pin.transform.position = this.transform.position;
                                pin.transform.rotation = this.transform.rotation;
                                if (!recorder.recording)
                                    iTween.ScaleTo(pin, iTween.Hash("scale", Vector3.one * 0.5f, "easeType", iTween.EaseType.easeOutElastic));
                                Invoke("hidePin", 1.5f);
                                Invoke("walk", 0.1f);
                            }
                        }
                        else
                        {
                            //print(hit.transform.name);
                            startDragPos = currentDragPos;
                            currentDragPos = hit.point;
                            currentDragPos.y = startDragPos.y;
                        }

                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!IsPointerOverUIObject())
                {
                    dragging = false;
                    if (holdTime < 0.25f)
                    {
                        //tap
                    }
                    else
                    {
                        //hpld
                    }
                    holdTime = 0;
                }
            }

            if (dragging)
            {
                animal.stopWalking();
                animal.gameObject.transform.position = Vector3.Lerp(animal.gameObject.transform.position, currentDragPos + offset, Time.deltaTime * 10);
            }
        }
    }

    public void hidePin()
    {
        iTween.ScaleTo(pin,Vector3.zero,0.5f);
    }

    void walk()
    {
        animal.StartWalking();
    }
}
