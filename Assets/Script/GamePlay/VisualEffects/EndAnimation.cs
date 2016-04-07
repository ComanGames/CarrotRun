using System;
using System.Collections;
using Assets.Script.iTween;
using UnityEngine;

namespace Assets.Script.GamePlay.VisualEffects
{
    public class EndAnimation:MonoBehaviour,IUiAnimation
    {
        #region Variables

        public event Action AnimationDone;
        public float InfoPanelAnimationTime;
        public float GiftButtonAnimationTime;
        public float DoubleButtonAnimationTime;
        public float ButtonsPanelAnimationTime;
        public float FirstAnimationTimeOut;
        public float SecondAnimationTimeOut;
        public float ThirdAnimationTimeOut;
        public float FourthAnimationTimeOut;

        public RectTransform InfoPanel;
        public RectTransform GiftButton;
        public RectTransform DoubleButton;
        public RectTransform ButtonPanel;

        //Private
        private Vector3 _infoPanelPosition;
        private Vector3 _giftButtonPosition;
        private Vector3 _doubleButtonPosition;
        private Vector3 _buttonPanelPosition;
        private int _aniamationsDone;

        #endregion


        public void RunAnimation()
        {
            StartSettings();
            Invoke("StartAnimationGiftButton", FirstAnimationTimeOut);
            Invoke("StartAnimationDoubleButton", SecondAnimationTimeOut);
            Invoke("StartAnimationInfoPanel", ThirdAnimationTimeOut);
            Invoke("StartAnimationButtonPanel", FourthAnimationTimeOut);
        }

        private void StartSettings()
        {
            _aniamationsDone = 0;

            _infoPanelPosition = InfoPanel.localPosition;
            InfoPanel.localPosition = new Vector3(InfoPanel.localPosition.x, InfoPanel.localPosition.y + InfoPanel.rect.height);

            var leftBorder = Screen.width/-2;

            _giftButtonPosition = GiftButton.localPosition;
            GiftButton.localPosition = new Vector3(leftBorder - GiftButton.rect.width,GiftButton.localPosition.y);

            _doubleButtonPosition = DoubleButton.localPosition;
            DoubleButton.localPosition = new Vector3(leftBorder - DoubleButton.rect.width,DoubleButton.localPosition.y);

            _buttonPanelPosition = ButtonPanel.localPosition;
            ButtonPanel.localPosition = new Vector3(ButtonPanel.localPosition.x, ButtonPanel.localPosition.y-ButtonPanel.rect.height);
        }

        public void StartAnimationInfoPanel()
        {
            Hashtable ht = new Hashtable();
            ht.Add("from", InfoPanel.localPosition.y);
            ht.Add("to", _infoPanelPosition.y);
            ht.Add("time", InfoPanelAnimationTime);
            ht.Add("onupdate", "InfoPanelMover");
            ht.Add("easetype", ITween.EaseType.easeOutCubic);
            ht.Add("oncomplete", "AnimationEnd");
            ITween.ValueTo(gameObject, ht);
        }

        public void InfoPanelMover(float newPosition)
        {
            InfoPanel.localPosition = new Vector3(InfoPanel.localPosition.x, newPosition);
        }
        public void StartAnimationButtonPanel()
        {
            Hashtable ht = new Hashtable();
            ht.Add("from", ButtonPanel.localPosition.y);
            ht.Add("to", _buttonPanelPosition.y);
            ht.Add("time", ButtonsPanelAnimationTime);
            ht.Add("onupdate", "ButtonPanelMover");
            ht.Add("easetype", ITween.EaseType.easeOutCubic);
            ht.Add("oncomplete", "AnimationEnd");
            ITween.ValueTo(gameObject, ht);
        }

        public void ButtonPanelMover(float newPosition)
        {
            ButtonPanel.localPosition = new Vector3(ButtonPanel.localPosition.x, newPosition);
        }
        public void StartAnimationGiftButton()
        {
            Hashtable ht = new Hashtable();
            ht.Add("from", GiftButton.localPosition.x);
            ht.Add("to", _giftButtonPosition.x);
            ht.Add("time", GiftButtonAnimationTime);
            ht.Add("onupdate", "GiftButtonMover");
            ht.Add("easetype", ITween.EaseType.easeOutBack);
            ht.Add("oncomplete", "AnimationEnd");
            ITween.ValueTo(gameObject, ht);
        }

        public void GiftButtonMover(float newPosition)
        {
            GiftButton.localPosition = new Vector3(newPosition, GiftButton.localPosition.y);
        }
        public void StartAnimationDoubleButton()
        {
            Hashtable ht = new Hashtable();
            ht.Add("from", DoubleButton.localPosition.x);
            ht.Add("to", _doubleButtonPosition.x);
            ht.Add("time", DoubleButtonAnimationTime);
            ht.Add("onupdate", "DoubleButtonMover");
            ht.Add("easetype", ITween.EaseType.easeOutBack);
            ht.Add("oncomplete", "AnimationEnd");
            ITween.ValueTo(gameObject, ht);
        }

        public void DoubleButtonMover(float newPosition)
        {
            DoubleButton.localPosition = new Vector3(newPosition, DoubleButton.localPosition.y);
        }

        public void AnimationEnd()
        {
            _aniamationsDone++;
            if (_aniamationsDone >= 4)
                AnimationDone?.Invoke();
        }
    }
}