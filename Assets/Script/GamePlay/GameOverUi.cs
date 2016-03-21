using Assets.Script.DataManagment;
using Assets.Script.Menu.DataManagment;
using Assets.Script.ScoreMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay
{
	public class GameOverUi : MonoBehaviour
	{
		#region Variables
		public GameObject GameOverCanvas;
		public ScoreNode CurrentScore;
		public ScoreNode HightScore;
		public Text GameOverCoinsText;
		#endregion

		public void EnableGameOver()
		{
			
			GameOverCoinsText.text = GameData.Instance.CurrentGameinfo.Coins.ToString();
			GameOverCanvas.SetActive(true);
			SetCurrentScore();
			SetHighScore();
		}

		public void DisableGameOver()
		{
			
			GameOverCanvas.SetActive(false);
		}
		public void SetHighScore()
		{
			SetPanel(HightScore, GameData.GetTopGameInfo());
		}

		public void SetCurrentScore()
		{
			SetPanel(CurrentScore, GameData.Instance.LastScoreInfo);
		}
		private void SetPanel(ScoreNode scoreNode, ScoreInfo scoreInfo)
		{
			scoreNode.Name.text = scoreInfo.PlayerName;
			scoreNode.Score.text = scoreInfo.Score.ToString();
			scoreNode.CharacterIcon.sprite = GameData.GetCharacterIcon(scoreInfo.Character);
		}
	}
}