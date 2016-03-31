using Assets.Script.GamePlay.Participators;
using Assets.Script.SAnimation;
using UnityEngine;

namespace Assets.Script.GamePlay.Skills
{
	public class OneTimeSkillBase : MonoBehaviour, ISkillHero
	{
		#region Variables

		public bool IsActive => IsSkillActive;
		protected bool IsSkillActive;
		public int SkillValue { get; set; }

		#endregion

		public virtual void SetSkill()
		{
		}

		public bool ActivateSkill(float power, Hero hero, SHeroAnimator heroAnimator)
		{
			if(power>=SkillValue)
			{
				GameManager.IsSkillActive = IsSkillActive = true;
				GameManager.Instance.UpdateSkillValue(power-SkillValue);
				SkillStart();
				return true;
			}
			return false;
		}

		protected virtual void SkillStart()
		{

		}

		public virtual void ForceEnd()
		{
			GameManager.IsSkillActive = IsSkillActive = false;
		}

		public void Froze()
		{
		}

		public void Unfroze()
		{
		}
	}
}