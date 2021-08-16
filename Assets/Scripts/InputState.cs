using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : MonoBehaviour {
    
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown, up;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool Up { get { return up; } }
    public Vector2 downPos { get { return startTouch; } }
    public Vector2 upPos;
    public Vector2 currentPos;
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Update() {
        
        up = tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
        currentPos = Input.mousePosition;
        #region Standalone Inputs
        
        if (Input.GetMouseButtonDown(0)) {
            isDraging = true;
            startTouch = currentPos;
        }
        
        else if (Input.GetMouseButtonUp(0)) {
            tap = true;
            isDraging = false;
            upPos = currentPos;
        }
        
        #endregion

        #region Mobile Input
        if (Input.touches.Length > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
                tap = true;
                isDraging = false;
                upPos = Input.touches[0].position;
            }
        }
        #endregion
        //checking for just tap
        if (tap) {
            if ((startTouch-upPos).magnitude < 10 ) {
                up = true;
            }
        }
        
        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging) {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }
        
        //Did we cross the distance?
        if (swipeDelta.magnitude > 45) {
            //Which direction?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y)) {
                //Left or right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else {
                // Up or down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
            Reset();
        }
        
        
    }

     void Reset() {
         swipeDelta = Vector2.zero;
         isDraging = false;
     }
}

