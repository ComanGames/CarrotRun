using Assets.Script.DataManagment;
using Assets.Script.Menu.DataManagment;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Menu
{
	public class NamePanelSetter : MonoBehaviour
	{
		#region Variables

		public GameObject NamePanel;
		public GameObject PlayButton;
		public InputField PlayerFiledName;
		public Text PlayerPanelName;
		public Text PlayerPanelCoins;
		public Image PlayerPanelIcon;
		public LoadingManager Loading;
		#endregion
		public void Start()
		{
				if (GameData.Instance.IsFirstGame)
				{
					EnableChangeName();
					PlayButton.SetActive(false);
				}
				else
				{
					SetInfoPannel();
					DestoyeUnusedNameUi();
				}
		}

		public void EnableChangeName()
		{
			PlayerFiledName.text = GameData.Instance.CurrentGameinfo.CurrentName;
		    NamePanel.SetActive(true);
		}

		private void SetInfoPannel()
		{
			PlayerPanelName.text = GameData.Instance.CurrentGameinfo.CurrentName;
			PlayerPanelCoins.text = GameData.Instance.CurrentGameinfo.Coins.ToString();
			PlayerPanelIcon.sprite = GameData.GetCharacterIcon(GameData.Instance.CurrentGameinfo.CurrentCharacter);
		}


		public void ButtonAcceptName()
		{
			GameData.Instance.CurrentGameinfo.CurrentName = PlayerFiledName.text;
			FileManager.SaveGameRecord();
			SetInfoPannel();
			DestoyeUnusedNameUi();
		}

		private void DestoyeUnusedNameUi()
		{
			NamePanel.gameObject.SetActive(false);
		}
	}
}