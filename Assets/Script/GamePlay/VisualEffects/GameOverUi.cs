using Assets.Script.SAnimation;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay.VisualEffects
{
	public class GameOverUi : MonoBehaviour
	{
		#region Variables

	    public Text MeetCanvas;
	    public SAnimationUi Gift;
	    public SAnimationUi Meet; 
		#endregion
		public void EnableGameOver()
		{
		    gameObject.SetActive(true);
            Gift.StopAnimation();
            Meet.StopAnimation();
		    Gift.StartAnimation();
		    Meet.StartAnimation();
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