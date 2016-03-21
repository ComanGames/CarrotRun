using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay
{
	[RequireComponent(typeof(Text))]
	[RequireComponent(typeof(Animation))]
	public class StartAnimation : MonoBehaviour
	{

		#region Variables

		public event Action AnimationDone;
		private Text _animationText;
		private Animation _animation;
		private Queue<string> _animationQueue ;
		#endregion

		public void Start()
		{
			_animationText = GetComponent<Text>();
			_animation = GetComponent<Animation>();

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
				yield return WaitForAnimation(_animation);
			}
			_animationText.text = "";
			AnimationDone?.Invoke();
		}

		private IEnumerator WaitForAnimation(Animation animation)
		{
			do
			{
				yield return null;
			} while (animation.isPlaying);
		}

		private void SetTextAndStartAnimation(string text)
		{
			_animationText.text = text;
			_animation.Play();
		}
	}
}