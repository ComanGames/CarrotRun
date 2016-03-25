using System;
using System.Collections;
using Assets.Script.DataManagment;
using Assets.Script.Menu.DataManagment;
using Assets.Script.ObjectPool;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Assets.Script.GamePlay
{
    //Test of git merging tool
    //Hello Zheka
	[Serializable]
	public class Level
	{
		#region  Variables

		public GameObject[] Items;

		#endregion

		#region Proporties

		public int Count
		{
			// ReSharper disable once ConvertPropertyToExpressionBody
			get
			{
				if (Items == null)
					return 0;
				return Items.Length;
			}
		}

		#endregion

		public GameObject this[int index]
		{
			get { return Items[index]; }
			set { Items[index] = value; }
		}
	}

    public class GameManager : MonoBehaviour
    {
        #region  Variables

        public BlocksManager ManagerBlocks;
        public UserInterfaceManager ManagerUserInterface;
        public SpeedSettingContainer SpeedSetting;
        public static GameManager Instance;
        public static bool IsSkillActive;
        public float Skill;
        public Hero Character;
        public bool Over;
        public StartAnimation StartAnimationGame;
        private BlurOptimized _blur;

        #endregion

        #region Methods

        public void Awake()
        {
            if (Instance == null || Instance != this)
                Instance = this;
        }

        public void Start()
        {
            LaunchingGame();
            Invoke("RunStartAnimation",0.1f);
        }

        private void RunStartAnimation()
        {
            Invoke("PauseGame", 0.1f);
            StartAnimationGame.RunAnimation();
            if (SoundController.Instance != null)
                SoundController.Instance.PauseSound();
        }


        private void LaunchingGame()
        {
            _blur = GetComponent<BlurOptimized>();
            _blur.enabled = false;
            StartAnimationGame.AnimationDone += RePauseGame;
            if(SoundController.Instance!=null)
                StartAnimationGame.AnimationDone += SoundController.Instance.RePauseSound;
            _playerStartPosition = Character.transform.position;
            StartSettings();
            ManagerBlocks.Creator();
            ManagerUserInterface.Creator();
            _created = true;
            StartCoroutine(SpeedingUp());
            _isPaused = false;
            ResumeGameEffects();
            float speed = GetBlockManagerSpeed();
            ManagerBlocks.UpdateSpeed(speed);
        }

        public void Update()
        {
            if (_created && !Over && !_isPaused)
            {
                ManagerBlocks.BlocksUpdate();
                UpdateScore();
            }
        }

        public void TurnOffProblems()
        {
            ManagerBlocks.Stop();
        }

        public void TurnOnProblems()
        {
            ManagerBlocks.Reset();
        }

        public void DestroyAllProblems()
        {
            ManagerBlocks.RemoveMovebleProblems();
        }

        public void UpdateSpeed()
        {
            SpeedUp();
        }

        public void UpdateSkillValue(float skillValue)
        {
            if (skillValue < 0 || skillValue > 100)
                return;
            Skill = skillValue;
            ManagerUserInterface.UpdateSkill((byte) Skill);
        }

        private void UpdateScore()
        {
            _score += (ulong) Time.timeScale;
            _realScore = _score/10;
            if (Skill < 100 && !IsSkillActive)
            {
                float skillValue = Skill + Time.timeScale/20;
                UpdateSkillValue(skillValue);
            }
            ManagerUserInterface.UpdateScore((int) _realScore);
        }

        public void AddCoin(GameObject item)
        {
            _coins++;
            ManagerUserInterface.UpdateCoins(_coins);
            item.Recycle();
            Character.CoinSound();
            ManagerBlocks.RemoveCoin(item);
        }

        public void GameOver()
        {
            Over = true;
            OverGame();
            SaveScore();
            Character.transform.position = _playerStartPosition;
            Character.Stop();
            ManagerUserInterface.EnableGameOver();
        }

        public void RePauseGame()
        {
            _blur.enabled = false;
            UnFrozeGame();
            UnFrozeAnations();
            ResumeGameEffects();

        }

        public void PauseGame()
        {
            _blur.enabled = true;
            FrozeGame();
            FrozeAnimations();
            StopGameEffects();
        }

        public void ResetLevel()
        {
            Character.Reset();
            ManagerUserInterface.Reset();
            UnFrozeGame();
            ResumeGameEffects();
            StartSettings();
            ManagerBlocks.Reset();
            RunStartAnimation();
        }

        private void OverGame()
        {
            ManagerBlocks.Stop();
            StopAllCoroutines();
            FrozeAnimations();
            StopGameEffects();
        }

        private void StartSettings()
        {
            IsSkillActive = false;
            Time.timeScale = SpeedSetting.StartSpeed;
            Over = false;
            _isPaused = false;
            _created = true;
            _score = 0;
            _coins = 0;
            _realScore = 0;
            _score = 0;
            Skill = 0;
        }

        #region Private

        private void FrozeGame()
        {
            _isPaused = true;
            Character.Froze();
            _lastTimeScale = Time.timeScale;
            ManagerBlocks.Pause();
            StopAllCoroutines();
        }

        private void UnFrozeGame()
        {
            _isPaused = false;
            Character.UnForze();
            Time.timeScale = _lastTimeScale;
            ManagerBlocks.UnPause();
            StartCoroutine(SpeedingUp());
        }

        private void UnFrozeAnations()
        {
            Character.MyAnimator.StartAnimation();
            ManagerUserInterface.ResumeAllAnimations();
            ManagerBlocks.ResumeAllAnimations();
        }

        private void FrozeAnimations()
        {
            ManagerUserInterface.StopAllAnimations();
            ManagerBlocks.StopAllAnimations();
        }

        private void SpeedUp()
        {
            if (Time.timeScale + SpeedSetting.Acceleration < SpeedSetting.MaxTimeScale)
            {
                float speed = GetBlockManagerSpeed();
                ManagerBlocks.UpdateSpeed(speed);
                ManagerBlocks.AddNewProblem();
            }
        }

        private float GetBlockManagerSpeed()
        {
            return Time.timeScale/SpeedSetting.SpeedScale;
        }

        private void StopGameEffects()
        {
            if (SoundController.Instance != null)
                SoundController.Instance.StopGameEffects();
        }

        private void ResumeGameEffects()
        {
            if (SoundController.Instance != null)
                SoundController.Instance.ResumeGameEffects();
        }

        private void SaveScore()
        {
            ScoreInfo info = new ScoreInfo(GameData.Instance.CurrentGameinfo.CurrentName, _score/10);
            GameData.Instance.CurrentGameinfo.Coins += (ulong) _coins;
            GameData.AddNewScore(info);
        }

        private IEnumerator SpeedingUp()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpeedSetting.SpeedTimeOut);
                //Speeding Up
                if (!IsSkillActive)
                {
                    SpeedUp();
                }
                if (Over)
                    yield break;
            }
        }

        #endregion

        private bool _created;

        private ulong _score;

        private int _coins;

        private ulong _realScore;

        private Vector2 _playerStartPosition;

        private float _lastTimeScale;

        private bool _isPaused;

        #endregion
    }

    [Serializable]
	public class SpeedSettingContainer
	{
		#region  Variables

		public float StartSpeed;
		public float MaxTimeScale = 30;
		public float SpeedScale;
		public int SpeedingUpCount;
		public float SpeedTimeOut;
		public float Acceleration;

		#endregion
	}
}