using System.Collections;
using Assets.Script.iTween;
using Assets.Script.Menu.DataManagment;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay.Skills
{
	public class SkillBanana : TimeSkillBase
	{
		#region Variables

		public GameObject Rainbow;

		public Sprite BgImage;

		public int TimeAnimation = 7;

		public bool IsShaking;

		public Vector3 DeffaultPostion;

		private Transform _myRainbow;

		private Transform _objectToShake;

		#endregion

		public override void Froze()
		{
			base.Froze();
			_myRainbow?.GetComponent<ParticleSystem>()?.Pause();
			ITween.Pause(MyHero.gameObject);
		}

		public override void Unfroze()
		{
			base.Unfroze();
			_myRainbow?.GetComponent<ParticleSystem>()?.Play();
			ITween.Resume(MyHero.gameObject);
		}

		public override void ForceEnd()
		{
			base.ForceEnd();
			StopSkillEfects();
			SetForGame();
			IsForzed = false;
			if(MyHero!=null)
				ITween.Stop(MyHero.gameObject);
		}

		protected override void SkillEnd()
		{
			base.SkillEnd();
			StopSkillEfects();
			StopFly();
		}

		private void StopSkillEfects()
		{
			IsShaking = false;
			if (_myRainbow?.gameObject != null) Destroy(_myRainbow.gameObject);
			_myRainbow = null;
			SkillBg.BgImage.enabled = false;
			SoundController.Instance?.ChangeSoundTrack(1);
		}

		private void StopFly()
		{
			Hashtable ht = new Hashtable {{"easetype", ITween.EaseType.easeOutSine}, {"time", TimeAnimation/2}, {"position",DeffaultPostion} };
			ITween.MoveTo(MyHero.gameObject,ht);
			// ReSharper disable once PossibleLossOfFraction
			StartCoroutine("GoToGround",TimeAnimation/2);
		}

		public IEnumerator GoToGround(float time)
		{
			float timeToWait = time;
			while (timeToWait>0)
			{
				while (IsForzed)
					yield return null;
				timeToWait -= Time.deltaTime;
				yield return null;
			}
			while (IsForzed)
				yield return null;
			GoToSkillEndAnimation();
			SetForGame();
		}

		public override void SetSkill()
		{
			SkillTime = 20;
			SkillValue = 100;
		}

		private void SetForGame()
		{
			MyHero.Body.isKinematic = false;
			GameManager.Instance?.TurnOnProblems();
			GameManager.IsSkillActive = IsSkillActive = false;
		}

		protected override void SkillStart()
		{
			_timeValue = 0;
			GoToSkillAnimation();
			MakeFly();
			StartCoroutine("StartSettings",TimeAnimation*0.6f);
		}

		public void Update()
		{
			if (IsShaking&&!IsForzed)
				ShakeBanan();
		}

		public IEnumerator StartSettings(float time)
		{
			float timeToWait = time;
			while (timeToWait > 0 )
			{
				while(IsForzed)
					yield return null;
				timeToWait -= Time.deltaTime;
				yield return null;
			}
			while (IsForzed)
				yield return null;
			RainbowAndBg();
			GameManager.Instance?.TurnOffProblems();
				if (SoundController.Instance != null)
					SoundController.Instance.ChangeSoundTrack(2);
			IsShaking = true;
		}

		private void RainbowAndBg()
		{
			_myRainbow = Instantiate(Rainbow).transform;
			Image image = SkillBg.BgImage;
			image.sprite = BgImage;
			image.enabled = true;
		}

		private float _timeValue;
		private void ShakeBanan()
		{
			_timeValue += Time.deltaTime;
			var f = Mathf.Sin(_timeValue*3f)*0.35f;
			_objectToShake.localPosition = new Vector3(0.0f, f, 0.0f);
			if (_myRainbow != null)
				_myRainbow.position = _objectToShake.position;
		}

		private void MakeFly()
		{
			MyHero.Body.isKinematic = true;
			ITween.MoveTo(MyHero.gameObject, new Vector3(0, 1, 0), TimeAnimation);
			_objectToShake = MyHero.transform.GetChild(0).transform;
		}

		private void GoToSkillAnimation()
		{
			HeroAnimator.GoToAnimation("SkillStart");
			HeroAnimator.AddNextAnimation("Skill");
		}

		private void GoToSkillEndAnimation()
		{
			HeroAnimator.GoToAnimation("SkillOver");
			HeroAnimator.AddNextAnimation("Run");

		}
	}
}