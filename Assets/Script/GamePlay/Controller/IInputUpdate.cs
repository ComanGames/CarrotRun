using System;

namespace Assets.Script.GamePlay.Controller
{
    public interface IInputUpdate
    {
         event Action TapEvent;
         event Action SwipeUpEvent;
         event Action SwipeDownEvent;
         event Action SwipeLeftEvent;
         event Action SwipeRightEvent;
         event Action SwipeUpOverEvent;
         event Action SwipeDownOverEvent;
         event Action SwipeLeftOverEvent;
         event Action SwipeRightOverEvent;

        void InputUpdate();
    }
}