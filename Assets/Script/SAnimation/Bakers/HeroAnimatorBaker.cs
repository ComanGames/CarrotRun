#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Assets.Script.SAnimation.Bakers
{
	[Serializable]
	public class HeroStates
	{
        public Sprite[] RunSprites;
        public Sprite[] SlideDownSprites;
        public Sprite[] SlideUpSprites;
        public Sprite[] SlideSprites;
        public Sprite[] JumpUpSprites;
        public Sprite[] JumpDownSprites;
        public Sprite[] JumpSprites;

		public bool IsReady()
		{
			return IsNotNull() && IsNotEmpty();
		}

		private bool IsNotEmpty()
		{
			return RunSprites.Length != 0
				&& SlideDownSprites.Length != 0 
				&& SlideUpSprites.Length != 0 
				&& SlideSprites.Length != 0 
				&& JumpUpSprites.Length != 0 
				&& JumpDownSprites.Length != 0 
				&& JumpSprites.Length != 0;
		}

		private bool IsNotNull()
		{
			return RunSprites != null
				&& SlideDownSprites != null 
				&& SlideUpSprites != null 
				&& SlideSprites != null 
				&& JumpUpSprites != null 
				&& JumpDownSprites != null 
				&& JumpSprites != null;
		}
	}
	public class HeroAnimatorBaker : MonoBehaviour
	{
		#region Variables
	    public bool DestoryIteself;
        public bool Bake;
		public HeroStates StateOfHero;
		#endregion

        public void OnDrawGizmos()
        {
	        if (Bake)
	        {
		        if (StateOfHero != null && StateOfHero.IsReady())
		        {
			        SHeroAnimator sa = gameObject.AddComponent<SHeroAnimator>();
			        sa.AnimationAddresses = new AnimationAddress[7];
			        CLLContainer container = SerializationUtilits.SerialazingAnimation(StateOfHero.RunSprites);
			        sa.AnimationAddresses[0] = new AnimationAddress("Run", container, true);

			        container = SerializationUtilits.SerialazingAnimation(StateOfHero.JumpUpSprites);
			        sa.AnimationAddresses[1] = new AnimationAddress("JumpUp", container, false);

			        container = SerializationUtilits.SerialazingAnimation(StateOfHero.JumpDownSprites);
			        sa.AnimationAddresses[2] = new AnimationAddress("JumpDown", container, false);

			        container = SerializationUtilits.SerialazingAnimation(StateOfHero.JumpSprites);
			        sa.AnimationAddresses[3] = new AnimationAddress("Jump", container, true);

			        container = SerializationUtilits.SerialazingAnimation(StateOfHero.SlideUpSprites);
			        sa.AnimationAddresses[4] = new AnimationAddress("SlideUp", container, false);

			        container = SerializationUtilits.SerialazingAnimation(StateOfHero.SlideDownSprites);
			        sa.AnimationAddresses[5] = new AnimationAddress("SlideDown", container, false);

			        container = SerializationUtilits.SerialazingAnimation(StateOfHero.SlideSprites);
			        sa.AnimationAddresses[6] = new AnimationAddress("Slide", container, true);

			        sa.DeffaultAnimation = "Run";

			        Bake = false;
			        if (DestoryIteself)
				        DestroyImmediate(this);
		        }
		        else
		        {
			        Bake = false;
			        throw new ArgumentException("YOu have some problem with state","StateOfHero");
		        }
	        }
        }
		 
	}
}
#endif