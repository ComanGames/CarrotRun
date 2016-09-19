using System.Collections;
using Assets.Script.DataManagment;
using Assets.Script.GamePlay.Data_Containers;
using Assets.Script.GamePlay.Participators;
using Assets.Script.Menu.DataManagment;
using UnityEngine;

namespace Assets.Script.GamePlay
{
    public class GameManager : MonoBehaviour
    {
        #region  Variables
        public Ads GameAds;
        public SpeedSettingContainer SpeedSetting;
        public SceneBuilder Builder;
        public static GameManager Instance;
        public static bool IsSkillActive;
        public float Skill;
        public bool Over;
        private BlocksManager _managerBlocks;
        private UserInterfaceManager _managerUserInterface;
        private Hero _character;
        private ulong _score;
        private int _coins;
        private ulong _realScore;
        private float _lastTimeScale;
        private bool _isPaused;
        #endregion

        #region Methods

        public void EnableSkillBlur()
        {
                _managerUserInterface.SkillBlur.enabled = true;
            _managerUserInterface.SkillVignette.enabled = true;

        }
        public void DisableSkillBlur()
        {
                _managerUserInterface.SkillBlur.enabled = false;
            _managerUserInterface.SkillVignette.enabled = false;

        }
        public void Awake()
        {
            if (Instance == null || Instance != this)
                Instance = this;
            Builder.Build();
            _managerUserInterface = Builder.BuildUi();
            _character = Builder.BuildHero();
            _managerBlocks = Builder.BuildBlockManager();
            
        }

        public void Start()
        {
            LaunchingGame();
            RunStartAnimation();
            Invoke("PauseGame", Time.deltaTime);
        }

        private void RunStartAnimation()
        {
            _managerUserInterface.RunStartAnimation();
            if (SoundController.Instance != null)
                SoundController.Instance.PauseSound();
        }


        private void LaunchingGame()
        {
            StartSettings();
            StartCoroutine(SpeedingUp());
            _isPaused = false;
            float speed = GetBlockManagerSpeed();
            _managerBlocks.UpdateSpeed(speed);
        }

        public void Update()
        {
            if (_managerBlocks != null && !Over && !_isPaused)
            {
                _managerBlocks.BlocksUpdate();
                UpdateScore();
            }
        }

        public void TurnOffProblems()
        {
            _managerBlocks.Stop();
        }

        public void TurnOnProblems()
        {
            _managerBlocks.Reset();
        }

        public void DestroyAllProblems()
        {
            _managerBlocks.RemoveMovebleProblems();
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
            _managerUserInterface.UpdateSkill((byte) Skill);
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
            _managerUserInterface.UpdateScore((int)_realScore);
        }

        public void AddCoin(GameObject item)
        {
            _coins++;
            _managerUserInterface.UpdateCoins(_coins);
            _character.CoinSound();
            _managerBlocks.RemoveCoin(item);
        }

        public void GameOver()
        {
            Over = true;
            OverGame();
            SaveScore();
            Builder.Stop();
            _managerUserInterface.EnableGameOver();
        }

        public void RePauseGame()
        {
            UnFrozeGame();
            UnFrozeAnations();
            ResumeGameEffects();

        }

        public void PauseGame()
        {
            FrozeGame();
            FrozeAnimations();
            StopGameEffects();
        }

        public void ResetLevel()
        {
            UnFrozeGame();
            ResumeGameEffects();
            StartSettings();
            Builder.Reset();
            PauseGame();
            RunStartAnimation();
        }


        private void OverGame()
        {
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
            _managerUserInterface.Blur.enabled = true;
            _character.Froze();
            _lastTimeScale = Time.timeScale;
            _managerBlocks.Pause();
            StopAllCoroutines();
        }

        private void UnFrozeGame()
        {
            _managerUserInterface.Blur.enabled = false;
            _isPaused = false;
            _character.UnForze();
            Time.timeScale = _lastTimeScale;
            _managerBlocks.UnPause();
            StartCoroutine(SpeedingUp());
        }

        private void UnFrozeAnations()
        {
            _character.MyAnimator.StartAnimation();
            _managerUserInterface.ResumeAllAnimations();
            _managerBlocks.ResumeAllAnimations();
        }

        private void FrozeAnimations()
        {
            _managerUserInterface.StopAllAnimations();
            _managerBlocks.StopAllAnimations();
        }

        private void SpeedUp()
        {
            if (Time.timeScale + SpeedSetting.Acceleration < SpeedSetting.MaxTimeScale)
            {
                float speed = GetBlockManagerSpeed();
                _managerBlocks.UpdateSpeed(speed);
                _managerBlocks.AddNewProblem();
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

        public void DoubleScoreAds()
        {
           GameAds.ShowAds(DoublingScore); 
        }

        public void DoubleMeatAds()
        {
           GameAds.ShowAds(DoublingMeat); 
        }

        public void DoublingMeat()
        {
            int from = _coins;
            _coins *= 2;
            int to = _coins;
            _managerUserInterface.AnimateFinalCoins(from,to);
            SaveScore();
        }

        public void DoublingScore()
        {
            int from = (int)_score/10;
            _score *= 2;
            int to = (int)_score/10;
            _managerUserInterface.AnimateFinalScore(from,to);
            SaveScore();
        }
        #endregion
        #endregion
    }
}