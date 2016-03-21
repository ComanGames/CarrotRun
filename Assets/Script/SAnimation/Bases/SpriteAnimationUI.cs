using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.SAnimation.Bases
{
	[RequireComponent(typeof(Image))]
	public class SpriteAnimationUi : MonoBehaviour
	{
		#region  Variables
		public const int NormalFps = 25;
		public int Fps = 10;
		public bool UseTimeScale;
		public bool AutoStart;
		public bool PreLoad;

		public Action AnimationEnd
		{
			set
			{
				AnimationContainer.OnAnimationEnd += value;
			}
			get { return AnimationContainer.OnAnimationEnd; }
		}
		protected Image SRenderer;
		protected CircleLinkedList AnimationContainer;
		
		protected bool Loaded;
		public GameObject ThisGameObject
		{
			// ReSharper disable once ConvertPropertyToExpressionBody
			get { return gameObject; }
		}

		#endregion

		#region Methods

		public virtual void Start()
		{
				Starter();
		}

		public void Starter()
		{
			SRenderer = GetComponent<Image>();
			if (!Loaded)
			{
				LoadContainers();
			}
			if (AutoStart)
				StartAnimation();
		}

		public virtual void LoadContainers()
		{
		}



		public void StartAnimation()
		{
			if (!Loaded)
			{
				Starter();
			}
			StartCoroutine(UpdatingSprite());
		}

		public void ResetGlobalAnimation()
		{
			StopAllCoroutines();
			StartCoroutine(UpdatingSprite());
			ResetAnimation();
		}
		public void ResetAnimation()
		{
			StopAnimation();
			if (AnimationContainer != null)
			{
				AnimationContainer.Reset();
				SRenderer.sprite = AnimationContainer.FirstNode.GetSprite();
			}
		}

		public void StopAnimation()
		{
			StopAllCoroutines();
		}


		public void GoToNextFrame(int count)
		{
			if (AnimationContainer.OneSpriteAnimation)
				return;
			var tSprite = SRenderer.sprite;

			for (var i = count; i >= 1; i--)
				tSprite = AnimationContainer.Next();
			SRenderer.sprite = tSprite;
		}

		public IEnumerator UpdatingSprite()
		{
			while (true)
			{
				if (Fps <= 0)
					yield return new WaitForFixedUpdate();

				float f = Fps;
				if (UseTimeScale)
					f *= Time.timeScale;

				var countToSkip = (int)(Mathf.Floor(f / NormalFps));
				var fasting = ((f - (NormalFps * countToSkip)) / NormalFps) + 1;


				if (f < NormalFps)
				{
					countToSkip = 1;
					fasting = f / NormalFps;
				}

				GoToNextFrame(countToSkip);
				yield return new WaitForSeconds(1 / (NormalFps * fasting));
			}
			// ReSharper disable once FunctionNeverReturns
		}

		#endregion

	}
}