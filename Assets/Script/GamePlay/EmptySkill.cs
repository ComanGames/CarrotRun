using Assets.Script.GamePlay.Skills;
using Assets.Script.SAnimation;
using UnityEngine;

namespace Assets.Script.GamePlay
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
			Debug.Log("We set the skill");
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