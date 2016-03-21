using UnityEngine;
using Assets.Script.DataManagment;
using Assets.Script.Menu.DataManagment;


public class MenuUI : MonoBehaviour {
	
	public void RecordButtonClicked()
	{
		SceneManager.LoadRecords();
	}	
	public void StartButtonClicked()
	{
		SceneManager.LoadGame();
	}

	public void CharacterButtonClicked()
	{
		SceneManager.LoadCharactersSelector();
	}

	public void QuitButtonClicked()
	{
		SceneManager.QuitGame();
	}

}
