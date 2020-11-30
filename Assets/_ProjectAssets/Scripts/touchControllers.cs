using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchControllers : MonoBehaviour
{
    Vector3 startPoint, currentPoint, offset, scanPos;
    public Transform animal;
    private Vector2 startPos,currentPos,prevPos;
    private bool directionChosen;
    private Vector2 direction;
    public Vector3 targetScale= Vector3.one;
    private bool touching;
    private bool doubleTouch = false;

    const float pinchTurnRatio = Mathf.PI / 2;
    const float minTurnAngle = 0;

    const float pinchRatio = 1;
    const float minPinchDistance = 1;

    const float panRatio = 1;
    const float minPanDistance = 1;

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


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1 && !doubleTouch)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    // Record initial touch position.
                    case TouchPhase.Began:
                        currentPos = prevPos = startPos = touch.position;
                        direction = Vector2.zero;
                        directionChosen = false;
                        touching = true;
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        prevPos = currentPos;
                        currentPos = touch.position;
                        direction = currentPos - prevPos;
                        break;

                    // Report that a direction has been chosen when the finger is lifted.
                    case TouchPhase.Ended:
                        currentPos = prevPos = startPos = Vector2.zero;
                        
                        touching = false;
                        directionChosen = true;
                        break;
                }
            }
            pinchDistance = pinchDistanceDelta = 0;
            turnAngle = turnAngleDelta = 0;

            // if two fingers are touching the screen at the same time ...
            if (Input.touchCount == 2)
            {
                direction = Vector2.zero;
                doubleTouch = true;
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
            direction = Vector2.Lerp(direction, Vector2.zero, Time.deltaTime * 10);
            doubleTouch = false;
        }

        animal.position += new Vector3(direction.x/(0.1f * Screen.width),direction.y/(0.1f*Screen.height),0);
    }

    void LateUpdate()
    {
        float pinchAmount = 0;
        Quaternion desiredRotation = transform.rotation;

        if (Mathf.Abs(pinchDistanceDelta) > 0)
        { // zoom
            pinchAmount = pinchDistanceDelta;
        }

        if (Mathf.Abs(turnAngleDelta) > 0)
        { // rotate
            Vector3 rotationDeg = Vector3.zero;
            rotationDeg.y = turnAngleDelta;
            desiredRotation *= Quaternion.Euler(rotationDeg);
        }

        animal.transform.rotation *= desiredRotation;

        targetScale += Vector3.one * (0.1f * pinchAmount) * Time.deltaTime;
        targetScale.x = Mathf.Clamp(targetScale.x, 0.2f, 5f);
        targetScale.y = Mathf.Clamp(targetScale.y, 0.2f, 5f);
        targetScale.z = Mathf.Clamp(targetScale.z, 0.2f, 5f);

        if (animal.GetComponentInChildren<animalController>().name == "AnimalControllerpangolin(Clone)")
        {
            //print("we got a pangolin");
            animal.transform.localScale = 2 * targetScale;
        }
        else
        {
            animal.transform.localScale = targetScale;
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
