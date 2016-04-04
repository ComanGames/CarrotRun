using Assets.Script.GamePlay.Participators;
using Assets.Script.SAnimation;
using UnityEngine;

namespace Assets.Script.GamePlay.Skills
{
	internal class EmptySkill : ISkillHero
	{
		public bool IsActive => false;
		public int SkillValue { get; }

		public bool ActivateSkill(float power, Hero hero, SHeroAnimator heroAnimator)
		{
			Debug.Log("We ActivateSkill");
			return false;
		}

		public void SetSkill()
		{
		}

		public void Froze()
		{
		}

		public void Unfroze()
		{
		}

		public void ForceEnd()
		{
		}
	}
}