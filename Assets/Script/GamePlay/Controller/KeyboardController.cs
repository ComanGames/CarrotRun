using System;
using UnityEngine;

namespace Assets.Script.GamePlay.Controller
{
    public class KeyboardController:IInputUpdate
    {
        private bool _holdingUp;
        private bool _holdingDown;
        private bool _holdingLeft;
        private bool _holdingRight;

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

        public event Action TapEvent;
        public event Action SwipeUpEvent;
        public event Action SwipeDownEvent;
        public event Action SwipeLeftEvent;
        public event Action SwipeRightEvent;
        public event Action SwipeUpOverEvent;
        public event Action SwipeDownOverEvent;
        public event Action SwipeLeftOverEvent;
        public event Action SwipeRightOverEvent;
        public void InputUpdate()
        {
            KeyBoardEvents();
        }
    }
}