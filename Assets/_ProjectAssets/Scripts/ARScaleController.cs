using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ARScaleController : MonoBehaviour
{
    TextMeshProUGUI txt;
    const float pinchTurnRatio = Mathf.PI / 2;
    const float minTurnAngle = 0;

    const float pinchRatio = 1;
    const float minPinchDistance = 0;

    const float panRatio = 1;
    const float minPanDistance = 0;
    public Transform animal;
    animalController animalController;
    /// <summary>
    ///   The delta of the angle between two touch points
    /// </summary>
    static public float turnAngleDelta;
    /// <summary>
    ///   The angle between two touch points
    /// </summary>
    static public float turnAngle;

    /// <summary>
    ///   The delta of the distance between two touch points that were distancing from each other
    /// </summary>
    static public float pinchDistanceDelta;
    /// <summary>
    ///   The distance between two touch points that were distancing from each other
    /// </summary>
    static public float pinchDistance;

    /// <summary>
    ///   Calculates Pinch and Turn - This should be used inside LateUpdate
    /// </summary>
    // Start is called before the first frame update
    animalController spawnedAnimal;
    Vector3 targetScale = Vector3.one * 0.7f;
    private void Start()
    {
        txt = GameObject.Find("scaleInfo").GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!animal)
        {
            spawnedAnimal = GameObject.FindObjectOfType<animalController>();
            if (spawnedAnimal)
            {
                if (spawnedAnimal.gameObject.name == "AnimalControllerpangolin(Clone)")
                    targetScale = Vector3.one;
                animalController = spawnedAnimal.gameObject.GetComponent<animalController>();
                animal = spawnedAnimal.gameObject.transform;
                animal.transform.localScale = Vector3.one * 1f;
            }
        }
        if (Input.touchCount > 0)
        {
            
            pinchDistance = pinchDistanceDelta = 0;
            turnAngle = turnAngleDelta = 0;

            // if two fingers are touching the screen at the same time ...
            if (Input.touchCount == 2)
            {
                spawnedAnimal.stopWalking();             
                Touch touch1 = Input.touches[0];
                Touch touch2 = Input.touches[1];

                // ... if at least one of them moved ...
                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    // ... check the delta distance between them ...
                    pinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
                                                          touch2.position - touch2.deltaPosition);
                    pinchDistanceDelta = pinchDistance - prevDistance;

                    // ... if it's greater than a minimum threshold, it's a pinch!
                    if (Mathf.Abs(pinchDistanceDelta) > minPinchDistance)
                    {
                        pinchDistanceDelta *= pinchRatio;
                    }
                    else
                    {
                        pinchDistance = pinchDistanceDelta = 0;
                    }

                    // ... or check the delta angle between them ...
                    turnAngle = Angle(touch1.position, touch2.position);
                    float prevTurn = Angle(touch1.position - touch1.deltaPosition,
                                           touch2.position - touch2.deltaPosition);
                    turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);

                    float averageX = (touch1.position.x + touch2.position.x) / 2;
                    //try: difference in touch1 & touch 2, greater one (using abs) dictates the rotation val?
                    float prevAvX = ((touch1.position.x - touch1.deltaPosition.x) + (touch2.position.x - touch1.deltaPosition.x)) / 2;
                    // ... if it's greater than a minimum threshold, it's a turn!
                    if (Mathf.Abs(turnAngleDelta) > minTurnAngle)
                    {
                        turnAngleDelta *= pinchTurnRatio;
                        //turnAngleDelta *= prevAvX * Time.deltaTime;
                    }
                    else
                    {
                        turnAngle = turnAngleDelta = 0;
                    }
                }
            }
        }

        else
        {
            
        }

        
    }

    void LateUpdate()
    {
        float pinchAmount = 0;
        Quaternion desiredRotation = Quaternion.identity;

        if (animal)
        {
            if (Mathf.Abs(pinchDistanceDelta) > 0)
            {
                // zoom
                animalController.stopWalking();
                pinchAmount = pinchDistanceDelta;
            }

            if (Mathf.Abs(turnAngleDelta) > 0)
            {
                // rotate
                animalController.stopWalking();
                Vector3 rotationDeg = Vector3.zero;
                rotationDeg.y = turnAngleDelta;
                desiredRotation *= ( Quaternion.Euler(-0.5f *rotationDeg));
            }

            if (Input.GetKey(KeyCode.UpArrow)) targetScale += Vector3.one * (0.1f * Time.deltaTime);
            if (Input.GetKey(KeyCode.DownArrow)) targetScale -= Vector3.one * (0.1f * Time.deltaTime);

            if (animal)
            {
                
                animal.transform.rotation *= desiredRotation;
                targetScale += Vector3.one * (0.1f * pinchAmount) * (0.1f * Time.deltaTime);
                targetScale.x = Mathf.Clamp(targetScale.x, 0.2f, 1f);
                targetScale.y = Mathf.Clamp(targetScale.y, 0.2f, 1f);
                targetScale.z = Mathf.Clamp(targetScale.z, 0.2f, 1f);

                animal.transform.localScale = targetScale;
                txt.text = "SCALE:\n<size=140%> " + Mathf.Floor(targetScale.x * 100) + "%</size>";
            }

            else
            {
                txt.text = "no animal";
            }
        }
    }


    static private float Angle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 from = pos2 - pos1;
        Vector2 to = new Vector2(1, 0);

        float result = Vector2.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        if (cross.z > 0)
        {
            result = 360f - result;
        }

        return result;
    }



}
