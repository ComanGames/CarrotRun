using UnityEngine;

namespace Assets.Script.GamePlay.VisualEffects
{
	public class GameOverUi : MonoBehaviour
	{
		#region Variables
		#endregion

		public void EnableGameOver()
		{
			gameObject.SetActive(true);
			SetCurrentScore();
			SetHighScore();
		}

		public void DisableGameOver()
		{
			gameObject.SetActive(false);
		}
		public void SetHighScore()
		{
		}

		public void SetCurrentScore()
		{
		}
	}
}