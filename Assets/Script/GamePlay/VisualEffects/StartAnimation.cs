using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.iTween;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay.VisualEffects
{
	[RequireComponent(typeof(Text))]
	public class StartAnimation : MonoBehaviour
	{

		#region Variables

		public event Action AnimationDone;
        public float TimeOut;
	    public float Scale=1.3f;
		private Text _animationText;
		private Queue<string> _animationQueue ;
	    private RectTransform _thisRectTransform;
	    private bool _isAnimationDone;
		#endregion

		public void Start()
		{
			_animationText = GetComponent<Text>();
		    _thisRectTransform = GetComponent<RectTransform>();
		}

		public void RunAnimation()
		{
			_animationQueue = new Queue<string>();
			_animationQueue.Enqueue("3");
			_animationQueue.Enqueue("2");
			_animationQueue.Enqueue("1");
			_animationQueue.Enqueue("Run");
			StartCoroutine(AnimationQueue());
		}

		private IEnumerator AnimationQueue()
		{
			while (_animationQueue.Count > 0)
			{
				SetTextAndStartAnimation(_animationQueue.Dequeue());
				yield return new WaitWhile(()=>!_isAnimationDone);
			}
			_animationText.text = "";
			AnimationDone?.Invoke();
		}

		private void SetTextAndStartAnimation(string text)
		{
		    _isAnimationDone = false;
		    _animationText.text = text;
		    AnimationUpStarter();
		}

	    public void AnimationUpStarter()
	    {
	        Hashtable ht = new Hashtable();
	        ht.Add("from", 1);
	        ht.Add("to", Scale);
	        ht.Add("time",TimeOut);
	        ht.Add("onupdate", "AnimationScale");
            ht.Add("easetype", ITween.EaseType.easeOutSine);
            ht.Add("oncomplete", "AnimationUpStarter");
	        ITween.ValueTo(gameObject, ht);
	    }

	    public void AnimationEnd()
	    {
	        _isAnimationDone = true;
	    }
        public void AnimationScale(float newValue)
	    {
            _thisRectTransform.localScale = Vector3.one*newValue;
	    }
	}
}