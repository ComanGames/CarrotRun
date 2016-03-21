using System;
using Assets.Script.DataManagment;
using Assets.Script.Menu.DataManagment;
using UnityEngine;

namespace Assets.Script.ScoreMenu
{
	[Serializable]

	public class ScoreMenu : MonoBehaviour
	{
		#region Variables
		public Transform ScoreTablet;
		public GameObject NodePrefab;
		#endregion

		public void Start()
		{
			//First we sort them
			GameData.SortPlayerRecords();
			//Now we add each one
			for (int i = 0; i < GameData.Instance.PlayerRecords.Count; i++)
			{
				Transform node = Instantiate(NodePrefab).transform;
				node.SetParent(ScoreTablet);
				RectTransform rectTransform = node.GetComponent<RectTransform>();
				Vector3 position = new Vector3(0, (i * rectTransform.localScale.y) - (ScoreTablet.localScale.y)) * -100f;
				rectTransform.localPosition = position;

				node.gameObject.GetComponent<ScoreNode>().SetNameAndScore(
					String.Format("{0} {1}", (i + 1), GameData.Instance.PlayerRecords[i].PlayerName),
					GameData.Instance.PlayerRecords[i].Score.ToString(),
					GameData.GetCharacterIcon(GameData.Instance.PlayerRecords[i].Character)
					);
				//some text
			}
		}

		public void ButtonMenuClicked()
		{
			SceneManager.LoadMenu();
		}
		public void ButtonCharactersClicked()
		{
			SceneManager.LoadCharactersSelector();
		}

		public void ButtonPlayAgainClicked()
		{
			SceneManager.LoadGame();
		}

	}
}