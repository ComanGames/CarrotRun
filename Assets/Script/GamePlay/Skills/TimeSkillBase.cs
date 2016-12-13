using System.Collections;
using Assets.Script.GamePlay.Participators;
using Assets.Script.SAnimation;
using Script.GamePlay.Participators;
using UnityEngine;

namespace Assets.Script.GamePlay.Skills
{
	public class TimeSkillBase:MonoBehaviour,ISkillHero
	{
		protected Hero MyHero;
		protected SHeroAnimator HeroAnimator;
		public bool IsActive => IsSkillActive;
		public int SkillValue { get; set; }
		protected bool IsSkillActive;
		protected float SkillTime = 5f;
		private float _timeToEnd;
		private float _timeToWait;
		private float _power;
		private float _timeLeft;
		protected bool IsForzed;

		public virtual void SetSkill()
		{

		}

		public bool ActivateSkill(float power,Hero hero,SHeroAnimator heroAnimator)
		{
			MyHero = hero;
			HeroAnimator = heroAnimator;

			if (power >= SkillValue)
			{
				GameManager.IsSkillActive = IsSkillActive = true;
				_power = power;
				_timeToWait = SkillTime/SkillValue;
				_timeToEnd = Time.time + SkillTime;
				SkillStart();
				StartCoroutine(SkillControlling());
				return true;
			}
			return false;
		}

		public virtual void ForceEnd()
		{
			StopAllCoroutines();
			GameManager.IsSkillActive = false;
			IsSkillActive = false;
			_power = 0;
			GameManager.Instance.UpdateSkillValue(0);
		}

		public virtual void Froze()
		{
			_timeLeft = _timeToEnd - Time.time;
			IsForzed = true;
		}

		public virtual void Unfroze()
		{
			_timeToEnd = Time.time + _timeLeft;
			IsForzed = false;
		}

		protected virtual void SkillStart()
		{
			
		}

		protected virtual void SkillEnd()
		{
		}

		private IEnumerator SkillControlling()
		{
			while (Time.time<_timeToEnd)
			{
			while (IsForzed)
				yield return new WaitForFixedUpdate();
				ChangeSkillValue();
				yield return new WaitForSeconds(_timeToWait);
			}
			while (IsForzed)
				yield return new WaitForFixedUpdate();
			SkillEnd();
		}

		private void ChangeSkillValue()
		{
			_power--;
			GameManager.Instance.UpdateSkillValue(_power);
		}
	}
}