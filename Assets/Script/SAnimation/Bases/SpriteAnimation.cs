using System.Collections;
using UnityEngine;

namespace Assets.Script.SAnimation.Bases
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteAnimation : MonoBehaviour
	{
		#region  Variables
		public const int NormalFps = 25;
		public int Fps = 10;
		public bool UseTimeScale;
		public bool AutoStart;
		public bool PreLoad;
		protected SpriteRenderer SRenderer;
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
			SRenderer = GetComponent<SpriteRenderer>();
		}

		public void Starter()
		{
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
				ResetConatriner();
			StartCoroutine(UpdatingSprite());
		}

		private void ResetConatriner()
		{
			AnimationContainer.Reset();
			if (SRenderer != null) SRenderer.sprite = AnimationContainer.FirstNode.GetSprite();
		}


		public virtual void StopAnimation()
		{
			StopAllCoroutines();
		}


		public void GoToNextFrame(int count)
		{
			if (AnimationContainer == null)
				LoadContainers();
			if (AnimationContainer != null && AnimationContainer.OneSpriteAnimation)
				return;
			if (SRenderer != null)
			{
				var tSprite = SRenderer.sprite;

				for (var i = count; i >= 1; i--)
					if (AnimationContainer != null) tSprite = AnimationContainer.Next();
				SRenderer.sprite = tSprite;
			}
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