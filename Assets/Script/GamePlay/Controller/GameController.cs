using System;
using UnityEngine;

namespace Assets.Script.GamePlay.Controller
{
    public class GameController : MonoBehaviour,IInputUpdate
    {

        public event Action TapEvent;

        public event Action SwipeUpEvent;

        public event Action SwipeDownEvent;

        public event Action SwipeLeftEvent;

        public event Action SwipeRightEvent;

        public event Action SwipeUpOverEvent;

        public event Action SwipeDownOverEvent;

        public event Action SwipeLeftOverEvent;

        public event Action SwipeRightOverEvent;

        private IInputUpdate _inputController;

        #region Variables

        #endregion

        public void Start()
        {
            
#if UNITY_EDITOR || UNITY_STANDALONE_WIN||UNITY_STANDALONE
            _inputController = new KeyboardController();
#endif
#if UNITY_ANDROID&&!UNITY_EDITOR
            _inputController = new TouchController();
        #endif
            _inputController.TapEvent += TapEvent;
            _inputController.SwipeUpEvent += SwipeUpEvent;
            _inputController.SwipeDownEvent +=  SwipeDownEvent;
            _inputController.SwipeLeftEvent +=  SwipeLeftEvent;
            _inputController.SwipeRightEvent +=  SwipeRightEvent;
            _inputController.SwipeUpOverEvent +=  SwipeUpOverEvent;
            _inputController.SwipeDownOverEvent +=  SwipeDownOverEvent;
            _inputController.SwipeLeftOverEvent +=  SwipeLeftOverEvent;
            _inputController.SwipeRightOverEvent +=  SwipeRightOverEvent;
        }
        #region Methods

	    public void Update()
	    {
            InputUpdate();
		}

        public void InputUpdate()
        {
            if(_inputController!=null)
                _inputController.InputUpdate();
        }

        #endregion

    }
}
