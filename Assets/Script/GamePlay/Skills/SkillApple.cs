using Assets.Script.GamePlay.VisualEffects;
using UnityEngine;

namespace Assets.Script.GamePlay.Skills
{
	public class SkillApple : OneTimeSkillBase
	{
		public SAnimation.SAnimation LazerAnimation;
		public SpriteRenderer LazerImage;
		public Color SkillColor;
		protected override void SkillStart()
		{
			LazerImage.enabled = true;
			LazerAnimation.StartAnimation();
			LazerAnimation.AnimationEnd += Btoom;
		}

		public override void SetSkill()
		{
			base.SetSkill();
			SkillValue = 50;
			SkillBg.BgImage.color = SkillColor;
		}

		private void Btoom()
		{
			LazerAnimation.StopAnimation();
			LazerImage.enabled = false;
			GameManager.Instance?.DestroyAllProblems();
			SkillBg.BgImage.enabled = true;
			Invoke("ForceEnd",0.5f);
		}


		public override void ForceEnd()
		{
			base.ForceEnd();	
			SkillBg.BgImage.enabled = false;
		}
	}
}