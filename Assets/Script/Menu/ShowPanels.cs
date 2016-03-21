using Assets.Script.DataManagment;
using Assets.Script.Menu.DataManagment;
using UnityEngine;

namespace Assets.Script.Menu.Scripts
{
	public class ShowPanels : MonoBehaviour {

		public GameObject OptionsPanel;							//Store a reference to the Game Object OptionsPanel 
		public GameObject MenuPanel;							//Store a reference to the Game Object MenuPanel 


		//Call this function to activate and display the Options panel during the main menu
		public void ShowOptionsPanel()
		{
			SceneManager.CurrentLoadingManager.StartTransactionOut(false);
			SceneManager.CurrentLoadingManager.TranactionOutEnd += ActivateOptionsPanel;
		}

		public  void ActivateOptionsPanel()
		{
			OptionsPanel.SetActive(true);
			SceneManager.CurrentLoadingManager.TranactionOutEnd -= ActivateOptionsPanel;
			SceneManager.CurrentLoadingManager.StartTransactionIn();
		}

		//Call this function to deactivate and hide the Options panel during the main menu
		public void HideOptionsPanel()
		{
			SceneManager.CurrentLoadingManager.StartTransactionOut(false);
			SceneManager.CurrentLoadingManager.TranactionOutEnd += DeactivateOptionsPanel;
		}

		private void DeactivateOptionsPanel()
		{
			OptionsPanel.SetActive(false);
			SceneManager.CurrentLoadingManager.TranactionOutEnd -= DeactivateOptionsPanel;
			SceneManager.CurrentLoadingManager.StartTransactionIn();
		}

		//Call this function to activate and display the main menu panel during the main menu
		public void ShowMenu()
		{
			MenuPanel.SetActive (true);
		}

		//Call this function to deactivate and hide the main menu panel during the main menu
		public void HideMenu()
		{
			MenuPanel.SetActive (false);
		}
	
		//Call this function to activate and display the Pause panel during game play

		//Call this function to deactivate and hide the Pause panel during game play
	}
}

