using Assets.Script.DataManagment;
using Assets.Script.GamePlay.VisualEffects;
using Assets.Script.Menu.DataManagment;
using Assets.Script.SAnimation.Bases;
using UnityEngine;
using UnityEngine.UI;

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
		public Sprite[] SkillSprites;
		public GameObject GameOverVideo;
		public SpriteAnimationUi JuiceAnimation;
		public AudioSource JuiceSound;
		public SAnimation.SAnimation[] Animations;

		#endregion

		#region Methods
		public void ButtonPlayAgain()
		{
			GameManager.Instance.ResetLevel();
			DisableGameOver();
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

		public void Creator()
		{
			DisableGameOver();
		}

		public void UpdateScore(int score)
		{
			ScoreCanvas.text = score.ToString();//Set score
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
	}

}