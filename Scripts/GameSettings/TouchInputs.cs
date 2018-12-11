using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchInputs : MonoBehaviour
{
    public static TouchInputs GC;
    /// <summary>
    /// goes between 0-1 
    /// </summary>
    float distanceFromEdge = 0.2f;
    public static bool swipedAtTop, swipedAtBottom, swipedAtRight, swipedAtLeft;

    Touch[] touches;
    InGameDebug debug;
    GameObject fingerCheck;
    

    void Start ()
    {
        GC = GetComponent<TouchInputs>();
        debug = InGameDebug.GC;
        touches = new Touch[10];
	}

	void Update ()
    {
        //just some Debugging.
        Vector3 tapPositionA;
        
        if (Input.touchCount > 0)
        {
            touches = Input.touches;
            tapPositionA = TapPosition(0);
            debug.NewDebug(tapPositionA.ToString(), "TapPosA");
            debug.NewDebug(Input.touchCount.ToString(), "TouchCount");
        }
        else
        {
            debug.NewDebug(Vector3.zero.ToString(), "TapPosA");
            debug.NewDebug("0", "TouchCount");
            touches.Initialize();
        }

        //debugging end

        SwipedOnEdge();
    }

    #region touchInfo
    public bool TapStationary(int finger)
    {
        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            return touches[finger].phase == TouchPhase.Stationary;
        }
        return false;
    }

    public bool TapMoved(int finger)
    {
        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            return touches[finger].phase == TouchPhase.Moved;
        }
        
        return false;
    }

    public bool TapBegan(int finger)
    {
        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            return touches[finger].phase == TouchPhase.Began;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }

    public bool TapEnded(int finger)
    {
        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            return touches[finger].phase == TouchPhase.Ended || touches[finger].phase == TouchPhase.Canceled;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            return true;
        }
        return false;
    }

    public bool TapCanceled(int finger)
    {
        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            return touches[finger].phase == TouchPhase.Canceled;
        }
        return false;
    }

    /// <summary>
    /// converts touch position from screen to world space.
    /// </summary>
    /// <param name="finger"></param>
    /// <returns></returns>

    public Vector3 TapPosition(int finger)
    {
        Vector3 tapPosition;
        Vector3 convertedPosition;
        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            tapPosition = new Vector3(touches[finger].position.x, touches[finger].position.y, -Camera.main.transform.position.z);
            convertedPosition = Camera.main.ScreenToWorldPoint(tapPosition);

            return convertedPosition;
        }
        else
        {
            tapPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            convertedPosition = Camera.main.ScreenToWorldPoint(tapPosition);

            return convertedPosition;
        }
        
        return Vector3.zero;
    }

    public GameObject ObjectOnTap(int finger)
    {
        
        Vector3 ray = TapPosition(finger);

        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            RaycastHit2D hit = Physics2D.Raycast(ray, touches[finger].position);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(ray, Input.mousePosition);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
    #endregion

    #region Gestures

    /// <summary>
    /// returns -1,0, or 1
    /// </summary>
    /// <returns></returns>

    public int RotatingDirection()
    {
        if (Input.touchCount > 1)
        {
            touches = Input.touches;
            Vector3 deltaPos = touches[1].deltaPosition;
            Vector3 tapPosA = touches[0].position;
            Vector3 tapPosB = touches[1].position;
            Vector3 diff = tapPosB - tapPosA;
           
            if (deltaPos.x > 0  && diff.y >= 0)
            {
                return 1;
            }
            else if (deltaPos.x < 0 && diff.y <= 0)
            {
                return 1;
            }
            else if (deltaPos.x < 0 && diff.y >= 0)
            {
                return -1;
            }
            else if (deltaPos.x > 0 && diff.y <= 0)
            {
                return -1;
            }
            

        }
        return 0;
    }

    public Vector3 SwipeVector(int finger)
    {
        if (Input.touchCount > finger)
        {
            touches = Input.touches;
            Vector3 deltaPos = touches[finger].deltaPosition;
            return deltaPos;
        }
        return Vector3.zero;
    }

    public int OnEdge(float distance)
    {
        if (distance >= 1)
        {
            distance = 1;
        }
        if (distance <= 0)
        {
            distance = 0;
        }

        if (Input.touchCount > 0)
        {
            touches = Input.touches;
            
            Vector3 touchPos = Camera.main.WorldToViewportPoint(TapPosition(0));
            print(touchPos);
            if (touchPos.x >= 1 - distance)
            {
                return 1;
            }
            if (touchPos.x <= 0 + distance)
            {
                return 2;
            }

            if (touchPos.y >= 1 - distance)
            {
                return 3;
            }
            if (touchPos.y <= 0 + distance)
            {
                return 4;
            }
            
        }
        return 0;
    }

    void SwipedOnEdge()
    {
        bool l = false, r = false, t = false, b = false;
        float sensitivity = 10f;
        switch (OnEdge(distanceFromEdge))
        {
            case 0:
                break;
            case 1:
                if (SwipeVector(0).x < -sensitivity)
                {
                    r = true;
                }
                break;
            case 2:
                if (SwipeVector(0).x > sensitivity)
                {
                    l = true;
                }
                break;
            case 3:
                if (SwipeVector(0).y < -sensitivity)
                {
                    t = true;
                }
                break;
            case 4:
                if (SwipeVector(0).y > sensitivity)
                {
                    b = true;
                }
                break;
        }

        swipedAtRight = r;
        swipedAtLeft = l;
        swipedAtTop = t;
        swipedAtBottom = b;
    }
    #endregion
}
