using System;
using System.Collections;
using Assets.Script.iTween;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay.VisualEffects
{
	public class StartAnimation : MonoBehaviour
	{

		#region Variables
		public event Action AnimationDone;
	    public float TimeOut;
	    public float BackTime;
	    public float Scale=1.3f;
	    public float SkillAnimationTime = 1.0f;
	    public float InfoPanelAnimationTime = 1.0f;
	    public float ButtonAnimationTime = 1.0f;
	    public GameObject TapToText;
	    public RectTransform RunText;
	    public RectTransform SkillPanel;
	    public RectTransform InfoPanel;
	    public Image PauseButton;
	    private int _aniamationsDone;
	    private Vector3 _skillPosition;
	    private Vector3 _infoPanelPosition;

	    #endregion

		public void RunAnimation()
		{
		    StartSettings();
		    AnimationStarter();
		}

	    private void StartSettings()
	    {
	        _aniamationsDone = 0;
	        _skillPosition = SkillPanel.localPosition;
	        SkillPanel.localPosition = new Vector3(SkillPanel.localPosition.x - SkillPanel.rect.width, SkillPanel.localPosition.y);
	        _infoPanelPosition = InfoPanel.localPosition;
	        InfoPanel.localPosition = new Vector3(InfoPanel.localPosition.x, InfoPanel.localPosition.y + InfoPanel.rect.height);
	        PauseButton.color = new Color(1, 1, 1, 0);
	    }

	    public void ScreenWasTaped()
        {
            TurnOffText();
            StartAnimationSkill();
            StartAnimationInfoPanel();
            StartAnimationPauseButton();
        }

	    private void StartAnimationPauseButton()
	    {
            Hashtable ht = new Hashtable();
            ht.Add("from", 0);
            ht.Add("to", 1);
            ht.Add("time", ButtonAnimationTime);
            ht.Add("onupdate", "ButtonAnimation");
            ht.Add("easetype", ITween.EaseType.easeOutCubic);
            ht.Add("oncomplete", "AnimationEnd");
            ITween.ValueTo(gameObject, ht);
        }
        public void ButtonAnimation(float newValue)
        {
            PauseButton.color = new Color(1,1,1,newValue);

        }
        private void StartAnimationInfoPanel()
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
        private void TurnOffText()
	    {
            RunText.gameObject.SetActive(false);
            TapToText.SetActive(false);
	    }

	    private void StartAnimationSkill()
	    {
            Hashtable ht = new Hashtable();
            ht.Add("from", SkillPanel.localPosition.x);
            ht.Add("to", _skillPosition.x);
            ht.Add("time", SkillAnimationTime);
            ht.Add("onupdate", "SkillMover");
            ht.Add("easetype", ITween.EaseType.easeOutCubic);
            ht.Add("oncomplete", "AnimationEnd");
            ITween.ValueTo(gameObject, ht);
        }
        public void SkillMover(float newPosition)
        {
            SkillPanel.localPosition = new Vector3(newPosition,SkillPanel.localPosition.y);
        }


        public void AnimationEnd()
        {
            _aniamationsDone++;
            if (_aniamationsDone == 3)
                AnimationDone?.Invoke();
        }

	    public void AnimationStarter()
	    {
	        Hashtable ht = new Hashtable();
	        ht.Add("from", 1);
	        ht.Add("to", Scale);
	        ht.Add("time",TimeOut);
	        ht.Add("onupdate", "AnimationScale");
            ht.Add("easetype", ITween.EaseType.easeOutCubic);
            ht.Add("oncomplete", "AnimationBack");
	        ITween.ValueTo(gameObject, ht);
	    }

	    public void AnimationBack()
	    {
            Hashtable ht = new Hashtable();
            ht.Add("from", Scale);
            ht.Add("to", 1);
            ht.Add("time", BackTime);
            ht.Add("onupdate", "AnimationScale");
            ht.Add("easetype", ITween.EaseType.easeInCubic);
            ht.Add("oncomplete", "AnimationStarter");
            ITween.ValueTo(gameObject, ht);

        }
        public void AnimationScale(float newValue)
	    {
            RunText.localScale = Vector3.one*newValue;
	    }
	}
}