using System;
using System.Collections;
using Assets.Script.GamePlay.Skills;
using Assets.Script.SAnimation;
using UnityEngine;

namespace Assets.Script.GamePlay
{
	[Serializable]
	public class HeroSounds
	{
		public AudioClip CoinSound;
		public AudioClip JumpSound;
		public AudioClip RunSound;
	}
	public class Hero : MonoBehaviour
	{
		#region  Variables
		//Public Variables 
		public Controller HeroController;
		//Jump Parameters
		public float JumpPower;
		public float JumpDistance = 1.0f;
		public float HighestPoint;
		//Slide Parameters
		public float SlideTime = 1.5f;
		public float SlideDistance = 2.1f;
		//Moving Parameters
		public float MaxPostionX = 3;
		public float BackSpeed = -0.3f;
		//System Settings
		public SHeroAnimator MyAnimator;
		public HeroSounds HeroSoundsCollection;
		public ISkillHero Skill;
		public Rigidbody2D Body;
		private bool _isFrozen = true;
		private AudioSource _audioSource;
		private AudioSource _audioSourceRun;
		private bool _isGrounded = true;
		private bool _isSlide;
		private bool _isMove;
		private float _prevVelocity;
		private PolygonCollider2D _standCollide;
		private PolygonCollider2D _slideCollide;
		private Vector2 _lastBodyVelocity;
		private bool _slideAfterGrounded;

		#endregion

		#region Methods

		public void SetHero()
		{
			SettControllerActions();
			_isFrozen = false;
			_audioSource = GetComponents<AudioSource>()[0];
			_audioSourceRun = GetComponents<AudioSource>()[1];
		    _audioSourceRun.clip = HeroSoundsCollection.RunSound;
		    _audioSourceRun.loop = true;
			Body = GetComponent<Rigidbody2D>();
			var polygons = MyAnimator.gameObject.GetComponents<PolygonCollider2D>();
			if (polygons[0] == null || polygons[1] == null)
				// ReSharper disable once NotResolvedInText
				throw new ArgumentNullException(@"You do not set collider for this character");
			_standCollide = polygons[1];
			_slideCollide = polygons[0];
			MadeStandColider();
			Skill.SetSkill();
		}

		private void SettControllerActions()
		{
			HeroController.TapEvent += TapPressed;
			HeroController.SwipeUpEvent += SwipeUp;
			HeroController.SwipeDownEvent += SwipeDown;
			HeroController.SwipeRightEvent += ActivateSkill;
		}

		private void ActivateSkill()
		{
			if (Skill != null && CanMove()) Skill.ActivateSkill(GameManager.Instance.Skill, this, MyAnimator);
		}

		private void SwipeDown()
		{
			if (CanMoveAndGrounded())
			{
				DoSlideDown();
			}
			else if (!_isFrozen && !_isSlide && !_isGrounded)
				_slideAfterGrounded = true;
		}

		private void OverSlide()
		{
			if (CanMoveAndGrounded() && !_isFrozen && _isSlide)
			{
				MyAnimator.EndSlide();
				_isSlide = false;
				MadeStandColider();
			}
			if (_slideAfterGrounded)
				_slideAfterGrounded = false;
		}

		private void DoSlideDown()
		{
			if (CanMove() && !_isSlide)
			{
				MyAnimator.Slide();
				_isSlide = true;
				MadeSlideColider();
				PlayJumpSound();
				StartCoroutine(SlideWork());
				Body.velocity = Vector2.zero;
				Vector2 distanceForward = ((Vector2.right * SlideDistance) / Time.timeScale);
				Body.AddForce( distanceForward, ForceMode2D.Impulse);
			}
		}

		private float _slideTime;
		private IEnumerator SlideWork()
		{
			_slideTime = SlideTime*Time.timeScale/Time.fixedDeltaTime;
			while (_slideTime > 0)
			{
				if (!_isFrozen)
					_slideTime -= Time.deltaTime*Time.timeScale;
				yield return 0;
			}
			OverSlide();

		}

		private bool CanMoveAndGrounded()
		{
			return CanMove() && _isGrounded;
		}

		private bool CanMove()
		{
		    if (Skill == null)
		        return !_isFrozen;
		    return !Skill.IsActive && !_isFrozen;
		}

		private void SwipeUp()
		{
			if (CanMoveAndGrounded())
			{
				Body.velocity = Vector2.zero;
				Vector2 distanceForward = ((Vector2.right * JumpDistance) / Time.timeScale);
				Body.AddForce(Vector2.up * JumpPower + distanceForward, ForceMode2D.Impulse);
				PlayJumpSound();
				MyAnimator.UpJump();
				_isGrounded = false;
				if (_isSlide)
					_isSlide = false;
			}
		}

		private void TapPressed()
		{
			if (CanMoveAndGrounded())
			{
			    if (!_isMove && transform.position.x < MaxPostionX)
			    {
			        Body.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
			        _isMove = true;
			    }
			}
		}


		public void FixedUpdate()
		{
			if (!_isFrozen)
			{
				if (Body.velocity.x > BackSpeed * Time.timeScale && Body.velocity.x < 0.5 && !_isSlide)
				{
					Body.AddForce(Vector2.left / 10, ForceMode2D.Impulse);
					_isMove = false;
				}
				//								if (transform.position.y > HighestPoint)
				//								{
				//									Body.velocity = Vector2.Lerp(Body.velocity, Vector2.down, 0.1f);
				//									MyAnimator.DownJump();
				//								}
				if (!_isGrounded && Mathf.Abs(Body.velocity.y) < 0.01f && _prevVelocity < 0 && !_isSlide)
				{
					_isGrounded = true;
					if (_slideAfterGrounded)
					{
					    DoSlideDown();
					    _slideAfterGrounded = false;
					}
					else
						MyAnimator.Run();
				}
				_prevVelocity = Body.velocity.y;
			}
		}


		private void MadeStandColider()
		{
			_standCollide.enabled = true;
			_slideCollide.enabled = false;
		}

		private void MadeSlideColider()
		{
			_standCollide.enabled = false;
			_slideCollide.enabled = true;
		}
		#endregion

		public void PlayJumpSound()
		{
			PlaySound(HeroSoundsCollection.JumpSound);
		}

		public void Reset()
		{
			UnForze();
			_isSlide = false;
			_isGrounded = true;
			_isMove = false;
			Body.velocity = Vector2.zero;
			MyAnimator.ResetAnimationGlobal();
			MyAnimator.Run();
			MadeStandColider();
		}

		public void Stop()
		{
			Froze();
			if (Skill != null && GameManager.IsSkillActive)
				Skill.ForceEnd();
			Body.velocity = Vector2.zero;
			_lastBodyVelocity = Vector2.zero;
			_prevVelocity = 0;
		}

		public void CoinSound()
		{
			PlaySound(HeroSoundsCollection.CoinSound);
		}

		private void PlaySound(AudioClip audioClip)
		{
			_audioSource.clip = audioClip;
			_audioSource.Play();
			_audioSource.loop = false;
		}

		public void Froze()
		{
            _audioSourceRun.Stop();
            if (Body != null)
			{
				_lastBodyVelocity = Body.velocity;
				_isFrozen = true;
				Body.constraints = RigidbodyConstraints2D.FreezeAll;
			}
			if (Skill != null && Skill.IsActive)
				Skill.Froze();
			MyAnimator.StopAnimation();
		}

		public void UnForze()
		{

            _audioSourceRun.Play();
			if (Body != null)
			{
				Body.constraints = RigidbodyConstraints2D.FreezeRotation;
				_isFrozen = false;
				Body.velocity = _lastBodyVelocity;
			}
			if (Skill != null && Skill.IsActive)
				Skill.Unfroze();

		}
	}
}