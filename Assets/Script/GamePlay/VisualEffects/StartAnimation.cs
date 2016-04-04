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
	    public float UpTime =3;
	    public float DownTime = 1;
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


		private IEnumerator WaitForAnimation(Animation animationToWait)
		{
			do
			{
				yield return null;
			} while (animationToWait.isPlaying);
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
	        ht.Add("from", -Screen.height/4);
	        ht.Add("to", Screen.height/2);
	        ht.Add("time",UpTime);
	        ht.Add("onupdate", "AnimationMove");
            ht.Add("easetype", ITween.EaseType.easeOutSine);
            ht.Add("oncomplete", "AnimationDownStarter");
	        ITween.ValueTo(gameObject, ht);
	    }

	    public void AnimationDownStarter()
	    {
            Hashtable ht = new Hashtable();
            ht.Add("from", Screen.height/2);
            ht.Add("to", -Screen.height/4);
            ht.Add("time", DownTime);
            ht.Add("easetype", ITween.EaseType.easeInExpo);
            ht.Add("onupdate", "AnimationMove");
            ht.Add("oncomplete", "AnimationEnd");
            ITween.ValueTo(gameObject, ht);
        }

	    public void AnimationEnd()
	    {
	        _isAnimationDone = true;
	    }
        public void AnimationMove(float newValue)
	    {
	        _thisRectTransform.position = new Vector3(_thisRectTransform.position.x,newValue);
	    }
	}
}