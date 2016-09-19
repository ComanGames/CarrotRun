using System.Collections;
using Assets.Script.GamePlay.VisualEffects;
using UnityEngine;

namespace Assets.Script.GamePlay.Skills
{
	public class SkillCarrot:TimeSkillBase
	{
		public Color SkillColor;
		public float TimeScale = 0.5f;
		private float _timeTimeScaleReal;

		public override void SetSkill()
		{
			SkillTime = 20;
			SkillValue = 100;
		}

		protected override void SkillStart()
		{
			GameManager.IsSkillActive = true;
            GameManager.Instance.EnableSkillBlur();
			IsSkillActive = false;
			_timeTimeScaleReal = Time.timeScale;
			StartCoroutine(Frozing());
		}

		public IEnumerator Frozing()
		{
			yield return null;
			SkillBg.BgImage.color = SkillColor;
			Time.timeScale = TimeScale;
			GameManager.Instance.UpdateSpeed();
		} 

		protected override void SkillEnd()
		{
			DisableSkillSettings();
		}

		private void DisableSkillSettings()
		{
            GameManager.Instance.DisableSkillBlur();
			GameManager.IsSkillActive = IsSkillActive = false;
			Time.timeScale = _timeTimeScaleReal;
			GameManager.Instance.UpdateSpeed();
		}

		public override void ForceEnd()
		{
			base.ForceEnd();
			DisableSkillSettings();
		}

	}
}