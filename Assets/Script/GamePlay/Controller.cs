using System;
using UnityEngine;

namespace Assets.Script.GamePlay
{
    public class Controller : MonoBehaviour
    {

	    #region Variables
	    public event Action TapEvent;
	    public event Action SwipeUpEvent;
	    public event Action SwipeDownEvent;
	    public event Action SwipeLeftEvent;
	    public event Action SwipeRightEvent;
	    public event Action SwipeUpOverEvent;
	    public event Action SwipeDownOverEvent;
	    public event Action SwipeLeftOverEvent;
	    public event Action SwipeRightOverEvent;
	    private bool _tapStarted;
	    private bool _holdingUp;
	    private bool _holdingDown;
	    private bool _holdingLeft;
	    private bool _holdingRight;
	    private float _distance;
	    private bool _hasSwiped;
	    private Touch _initialTouch;
	    #endregion

	    #region Methods

	    public void Update()
	    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN||UNITY_STANDALONE
		    KeyBoardEvents();
#endif
#if UNITY_ANDROID&&!UNITY_EDITOR
			
			TouchEvents();
#endif

		}

	    private void TouchEvents()
	    {
		    if (Input.touchCount < 1)
			    AllToFalse();
		    foreach (var t in Input.touches)
		    {
			    if (t.phase == TouchPhase.Began)
			    {
				    _initialTouch = t;
				    _tapStarted = true;
			    }
			    else if (t.phase == TouchPhase.Moved )
			    {
				    var deltaX = _initialTouch.position.x - t.position.x;
				    var deltaY = _initialTouch.position.y - t.position.y;
				    _distance = Mathf.Sqrt((deltaX*deltaX) + (deltaY*deltaY));
				    var swipedSideways = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);

				    if (_distance >25&&!_hasSwiped)
				    {
					    CheckForSwype(swipedSideways, deltaX, deltaY);
				    }
				}
			    else if (t.phase == TouchPhase.Ended)
			    {
				    _initialTouch = new Touch();
				    _hasSwiped = false;
				    AllToFalse();
				    if (_tapStarted)
					    if (TapEvent != null) TapEvent();
			    }
		    }
	    }

	    private void CheckForSwype(bool swipedSideways, float deltaX, float deltaY)
	    {
		    if (swipedSideways)
		    {
			    if (deltaX > 0) //swiped left
			    {
				    if (SwipeLeftEvent != null) SwipeLeftEvent();
				    _holdingLeft = true;
			    }
			    else if (deltaX < 0) //swiped right
			    {
				    if (SwipeRightEvent != null) SwipeRightEvent();
				    _holdingRight = true;
			    }
		    }
		    else
		    {
			    if (deltaY > 0) //swiped down
			    {
				    if (SwipeDownEvent != null) SwipeDownEvent();
				    _holdingDown = true;
			    }
			    else if (deltaY < 0) //swiped up
			    {
				    if (SwipeUpEvent != null) SwipeUpEvent();
				    _holdingUp = true;
			    }

			    _hasSwiped = true;
			    _tapStarted = false;
		    }
	    }

	    private void AllToFalse()
	    {

		    if (_holdingUp)
		    {
			    _holdingUp = false;
			    if (SwipeUpOverEvent != null) SwipeUpOverEvent();
		    }
		    if (_holdingDown)
		    {
			    _holdingDown = false;
			    if (SwipeDownOverEvent != null) SwipeDownOverEvent();
		    }
		    if (_holdingLeft)
		    {
			    if (SwipeLeftOverEvent != null) SwipeLeftOverEvent();
			    _holdingLeft = false;
		    }
		    if (_holdingRight)
		    {
			    if (SwipeRightOverEvent != null) SwipeRightOverEvent();
			    _holdingRight = false;
		    }
	    }

	    private void KeyBoardEvents()
	    {
		    //Up
		    if (Input.GetButtonDown("Up"))
		    {
			    if (SwipeUpEvent != null) SwipeUpEvent();
			    _holdingUp = true;
		    }
		    if (Input.GetButtonUp("Up") && _holdingUp)
		    {
			    _holdingUp = false;
			    if (SwipeUpOverEvent != null) SwipeUpOverEvent();
		    }

		    //Down
		    if (Input.GetButtonDown("Down"))
		    {
			    if (SwipeDownEvent != null) SwipeDownEvent();
			    _holdingDown = true;
		    }
		    if (Input.GetButtonUp("Down") && _holdingDown)
		    {
			    _holdingDown = false;
			    if (SwipeDownOverEvent != null) SwipeDownOverEvent();
		    }

		    //Left
		    if (Input.GetButtonDown("Left"))
		    {
			    if (SwipeLeftEvent != null) SwipeLeftEvent();
			    _holdingLeft = true;
		    }
		    if (Input.GetButtonUp("Left") && _holdingLeft)
		    {
			    if (SwipeLeftOverEvent != null) SwipeLeftOverEvent();
			    _holdingLeft = false;
		    }

		    //Right
		    if (Input.GetButtonDown("Right"))
		    {
			    if (SwipeRightEvent != null) SwipeRightEvent();
			    _holdingRight = true;
		    }
		    if (Input.GetButtonUp("Right") && _holdingRight)
		    {
			    if (SwipeRightOverEvent != null) SwipeRightOverEvent();
			    _holdingRight = false;
		    }
		    if (Input.GetKeyDown(KeyCode.Space))
			    if (TapEvent != null) TapEvent();
	    }

	    #endregion

    }
}
