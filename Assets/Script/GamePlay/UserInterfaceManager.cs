using Assets.Script.GamePlay.VisualEffects;
using Assets.Script.Menu.DataManagment;
using Assets.Script.SAnimation.Bases;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace Assets.Script.GamePlay
{

	public class UserInterfaceManager : MonoBehaviour
	{
		#region Variables
		public GameOverUi GameOverConvas;
		public Image SkillPanel;
		public Text ScoreCanvas;
	    public Text ScoreCanvas2;
		public Text CoinsCanvas;
		public Text CoinsCanvas2;
		public Text TopScoreCanvas;
		public Text TopScoreCanvas2;
		public Sprite[] SkillSprites;
		public GameObject GameOverVideo;
		public SpriteAnimationUi JuiceAnimation;
		public AudioSource JuiceSound;
		public SAnimation.SAnimation[] Animations;
	    // ReSharper disable once InconsistentNaming
	    public SkillBg SkillBG;
	    public BlurOptimized Blur;
        public StartAnimation StartAnimationGame;
	    public EndAnimation EndAnimationGame;
        //Score and coin animation
	    public NumberAnimaiton ScoreDoublingAnimaiton;
	    public NumberAnimaiton CoinDoublingAnimation;
		#endregion

		#region Methods

	    public void Creator()
	    {
            SkillBG.Create();
	        DisableGameOver();
            SetTopScore((int)GameData.GetTopGameInfo().Score);
            StartAnimationGame.AnimationDone += GameManager.Instance.RePauseGame;
            if(SoundController.Instance!=null)
                StartAnimationGame.AnimationDone += SoundController.Instance.RePauseSound;
        }

	    public void ButtonPlayAgain()
		{
			GameManager.Instance.ResetLevel();
			DisableGameOver();
		}

	    public void ButtonCharacterSelector()
		{
            SceneManager.LoadCharactersSelector();
		}

	    public void ButtonScoreTable()
		{
			SceneManager.LoadRecords();
		}

	    public void ButtonX2Score() { }

	    public void ButtonX2Coins() { }


	    public void ButtonGoToMenu()
		{
			SoundController.Instance?.RePauseSound();
			SceneManager.LoadMenu();
		}

	    public void UpdateScore(int score)
		{
			ScoreCanvas.text = score.ToString();//Set score
			ScoreCanvas2.text = score.ToString();//Set score
	        if (score > (int) GameData.GetTopGameInfo().Score)
                SetTopScore(score);
		}

	    private void SetTopScore(int count)
	    {
	        TopScoreCanvas.text = "TOP: "+count;
	        TopScoreCanvas2.text = "TOP: "+count;
	    }

	    public void UpdateSkill(byte skill)
		{
			SetSkillUi(skill);//set Skill 
		}

		private void SetSkillUi(byte skill)
		{
			float proportion = (float)(SkillSprites.Length) / 100;
			int n = (int)(skill * proportion);
			if (n > SkillSprites.Length - 1)
				return;
			SkillPanel.sprite = SkillSprites[n];
		}

		public void UpdateCoins(int coins)
		{
			CoinsCanvas.text = coins.ToString();
			CoinsCanvas2.text = coins.ToString();

        }

        public void EnableGameOver()
		{

			EnableGameOverVideo();
		}

		private void EnableGameOverVideo()
		{
			GameOverVideo.SetActive(true);
			JuiceAnimation.ResetAnimation();
			JuiceSound.Play();
			JuiceAnimation.StartAnimation();
			JuiceAnimation.AnimationEnd = EnableGameOverConvas;
		}

		private void EnableGameOverConvas()
		{
			GameOverConvas.EnableGameOver();
			JuiceSound.Stop();
		    JuiceAnimation.StopAnimation();
            EndAnimationGame.RunAnimation();
		}
        
		public void DisableGameOver()
		{
			GameOverVideo.SetActive(false);
			GameOverConvas.DisableGameOver();

		}

		public void Reset()
		{
			UpdateCoins(0);
			UpdateScore(0);
			UpdateSkill(0);
            SetTopScore((int)GameData.GetTopGameInfo().Score);
            ResetAllAnimations();
		}

		public void ResetAllAnimations()
		{
			foreach (SAnimation.SAnimation sAnimation in Animations)
				sAnimation.ResetGlobalAnimation();
		}

		public void StopAllAnimations()
		{

			foreach (SAnimation.SAnimation sAnimation in Animations)
				sAnimation.StopAnimation();
		}

		public void ResumeAllAnimations()
		{
			foreach (SAnimation.SAnimation sAnimation in Animations)
				sAnimation.StartAnimation();
		}

		#endregion
	    public  void RunStartAnimation()
	    {
            StartAnimationGame.RunAnimation();
	    }

	    public void AnimateFinalScore(int from, int to)
	    {
            ScoreDoublingAnimaiton.RunAnimation(2,from,to);
	    }

	    public void AnimateFinalCoins(int from, int to)
	    {
            CoinDoublingAnimation.RunAnimation(1, from, to);
        }
    }

}