using System;
using System.Collections.Generic;
using Assets.Script.SAnimation.Bakers;
using Assets.Script.SAnimation.Bases;
using UnityEngine;

namespace Assets.Script.SAnimation
{
	[Serializable]
	[RequireComponent(typeof(SpriteRenderer))]
	public class AnimationAddress
	{
		#region  Variables

		public string Name;
		public bool IsCircle;
		public CLLContainer Container;

		#endregion

		public AnimationAddress(string name, CLLContainer container, bool isCircle)
		{
			Name = name;
			Container = container;
			IsCircle = isCircle;
		}

		public AnimationAddress(string name, CLLContainer container)
		{
			Name = name;
			Container = container;
			IsCircle = true;
		}
	}

	public class SAnimator : SpriteAnimation
	{
		#region  Variables
		public string DeffaultAnimation;
		public Dictionary<string, CircleLinkedList> Animations;
		public AnimationAddress[] AnimationAddresses;
		private Queue<string> _animationQueue;
		private bool _autoGoToNext;
		#endregion

		#region Methods

		public override void LoadContainers()
		{
			if (AnimationAddresses == null || AnimationAddresses.Length == 0)
				throw new InvalidOperationException("We have no animation to preLoad");

			//Creating an objects
			Animations = new Dictionary<string, CircleLinkedList>(AnimationAddresses.Length);
			_animationQueue = new Queue<string>();

			//Making all containers
			LoadContainerAddress();

			//Getting what animation should be default
			if (DeffaultAnimation == default(string) || !Animations.ContainsKey(DeffaultAnimation))
				DeffaultAnimation = AnimationAddresses[0].Name;

			GoToAnimation(DeffaultAnimation); //Same as  AnimationContainer = Animations[DeffaultAnimation];
			Loaded = true;
		}

		private void LoadContainerAddress()
		{
			foreach (var t in AnimationAddresses)
			{
				CircleLinkedList loadAnimationContainer = t.Container.GetCircleLinkedList();
				if (loadAnimationContainer != null)
				{
					loadAnimationContainer.IsCircle = t.IsCircle;
					Animations.Add(t.Name, loadAnimationContainer);
				}
			}
		}


		public void GoToAnimation(string animationName)
		{
			ChangeAnimation(animationName);
			if(_animationQueue==null)
				_animationQueue = new Queue<string>();
			else
				_animationQueue.Clear();
			GoToNextFrame(1);
		}

		private void ChangeAnimation(string animationName)
		{
			if (!Animations.ContainsKey(animationName))
				throw new InvalidOperationException("The animation " + animationName + " isn't in over list");
			//Changing Animation
			AnimationContainer = Animations[animationName];
			ResetAnimation();
		}

		public void ResetAnimationGlobal()
		{
			if (_autoGoToNext)
			{
				_autoGoToNext = false;
				// ReSharper disable once DelegateSubtraction
				if (AnimationContainer.OnAnimationEnd != null) AnimationContainer.OnAnimationEnd -= GotoNextAnimation;
			}
			while (_animationQueue.Count!=0)
			{
				_animationQueue.Dequeue();	
			}
			StopAllCoroutines();
			StartCoroutine(UpdatingSprite());
		}
		public void AddNextAnimation(string animationName)
		{

			if (!_autoGoToNext)
				_autoGoToNext = true;
			_animationQueue.Enqueue(animationName);
			AnimationContainer.OnAnimationEnd += GotoNextAnimation;


		}

		private void GotoNextAnimation()
		{
			if (_animationQueue.Count > 0)
			{
				ChangeAnimation(_animationQueue.Dequeue());
			}
		}

		#endregion
	}
}