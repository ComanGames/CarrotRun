using Assets.Script.Menu.DataManagment;
using UnityEngine;

namespace Assets.Script.GamePlay.VisualEffects
{
	public class GamePlayPause : MonoBehaviour
	{
		#region Variables
        //Reference to Audio mixer snapshot in which the master volume of main mixer is turned up
		public GameObject SettingsButton;
		public GameObject PausePannel;
		private bool _isPauseEnable;
		#endregion

		public void Awake()
		{
			_isPauseEnable = false;
		}
		public  void DisableSettingsButton()
		{
			SettingsButton.SetActive(false);
		}

		public  void EnableSetttingsButton()
		{
			SettingsButton.SetActive(true);

		}

		public void EnablePause()
		{
			if (!_isPauseEnable)
			{

				SoundController.Instance?.PauseSound();
				if (GameManager.Instance != null)
					GameManager.Instance.PauseGame();
				PausePannel.SetActive(true);
				_isPauseEnable = true;
				DisableSettingsButton();
				
			}
		}

		public void DisablePause()
			{
			SoundController.Instance?.RePauseSound();
				if (GameManager.Instance != null)
				GameManager.Instance.RePauseGame();
			PausePannel.SetActive(false);
			_isPauseEnable = false;
			EnableSetttingsButton();
		} 
	}
}