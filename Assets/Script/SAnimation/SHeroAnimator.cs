using Assets.Script.DataManagment;
using Assets.Script.GamePlay;
using Assets.Script.Menu.DataManagment;
using UnityEngine;

namespace Assets.Script.SAnimation
{
	public class SHeroAnimator : SAnimator, IHeroController
	{
		public Object[] SpriteSheets;
		public Characters Character;

		public override void Start()
		{

			UseTimeScale = true;
			AutoStart = true;
			Character = GameData.Instance.CurrentGameinfo.CurrentCharacter;
			DeffaultAnimation = "Run";
			base.Start();
			if (ResourceUnloder.Instance != null && ResourceUnloder.Instance.ToUnlodad!=null)
				ResourceUnloder.Instance.ToUnlodad.AddRange(SpriteSheets);
		}



		public void UpJump()
		{
			GoToAnimation("JumpUp");
			AddNextAnimation("Jump");
		}

		public void DownJump()
		{
			GoToAnimation("JumpDown");
		}

		public void Run()
		{
			GoToAnimation("Run");

		}

		public void Slide()
		{
			GoToAnimation("SlideDown");
			AddNextAnimation("Slide");
		}

		public void EndSlide()
		{
			GoToAnimation("SlideUp");
			AddNextAnimation("Run");

		}
	}

	public interface IHeroController
	{

		void UpJump();
		void DownJump();
		void Run();
		void Slide();

	}
}