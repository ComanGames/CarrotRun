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
		public GameObject ChangeNameConvas;
		public InputField NewPalyerName;
		public Image SkillPanel;
		public Text ScoreCanvas;
		public Text CoinsCanvas;
		public Text TopScoreCanvas;
		public Sprite[] SkillSprites;
		public GameObject GameOverVideo;
		public SpriteAnimationUi JuiceAnimation;
		public AudioSource JuiceSound;
		public SAnimation.SAnimation[] Animations;
	    public SkillBg SkillBG;
	    public BlurOptimized Blur;
        public StartAnimation StartAnimationGame;
	    public Sprite JuiceLastSprite;
		#endregion

		#region Methods

	    public void Creator()
	    {
            SkillBG.Create();
	        DisableGameOver();
	        Blur.enabled = false;
	        TopScoreCanvas.text = GameData.GetTopGameInfo().Score.ToString();
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

	    public void ButtonChangeName()
		{
			NewPalyerName.text = GameData.Instance.CurrentGameinfo.CurrentName;
			ChangeNameConvas.SetActive(true);
		}

	    public void ButtonChangeNameAccept()
		{
			string newName = NewPalyerName.text;
			GameData.Instance.CurrentGameinfo.CurrentName = newName;
			GameData.Instance.LastScoreInfo.PlayerName = newName;
			GameOverConvas.SetCurrentScore();
			ButtonChangeNameBack();
		}

	    public void ButtonChangeNameBack()
		{
			ChangeNameConvas.SetActive(false);
		}

	    public void ButtonGoToMenu()
		{
			SoundController.Instance?.RePauseSound();
			SceneManager.LoadMenu();
		}

	    public void UpdateScore(int score)
		{
			ScoreCanvas.text = score.ToString();//Set score
            if(score>(int) GameData.GetTopGameInfo().Score)
            TopScoreCanvas.text = score.ToString();

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
            TopScoreCanvas.text = GameData.GetTopGameInfo().Score.ToString();
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

	    public  void RunAnimation()
	    {
            StartAnimationGame.RunAnimation();
	    }
	}

}