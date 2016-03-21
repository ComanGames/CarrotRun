using System;
using Assets.Script.SAnimation.Bakers;
using Assets.Script.SAnimation.Bases;
using UnityEngine;

namespace Assets.Script.SAnimation
{
	[RequireComponent(typeof(SpriteRenderer))]
    public class SAnimation : SpriteAnimation
    {
        #region Variables

        //Public Variables 
        public  CLLContainer Container;
		public  Action AnimationEnd;
        //private Variables
        #endregion
        public override void LoadContainers()
        {
			if(Container!=null)
			{
				AnimationContainer = Container.GetCircleLinkedList();
				AnimationContainer.OnAnimationEnd += AnimationEndAction;
				Loaded = true;
			}
        }

		private void AnimationEndAction()
		{
			AnimationEnd?.Invoke(); 
		}
    }
}