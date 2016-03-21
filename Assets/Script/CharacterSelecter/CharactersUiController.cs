using System;
using System.Collections;
using Assets.Script.DataManagment;
using Assets.Script.iTween;
using Assets.Script.Menu.DataManagment;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.CharacterSelecter
{
	public class CharactersUiController : MonoBehaviour
	{
		#region  Variables
		public GameObject BuyMorePanel;
		public Image CharacterIcon;
		public Sprite UnknownCharacterSprite;
		public Text PlayerNameText;
		public Text CoinsCountText;
		public Text SecondPriceText;
		public float CharacterAnimationTime;
		public BuyingButtonContainer BuyingContainer;
		public float AnimationShift = -40;
		public double AnimationTime = 5;
		private static CharactersUiController _instance;
		public static float DeffaultYPosition;
		private static Color _deffaultColor;
		public bool First;
		private Vector3 _deffaultPosition;

		#endregion

		public void Awake()
		{
			_instance = this;
			SecondPriceText.enabled = false;
		}

		public void Start()
		{
			HideMassageNotEnoughMone();
			SetPlyerInfo();
			_deffaultPosition = SecondPriceText.GetComponent<RectTransform>().position;
			DeffaultYPosition = SecondPriceText.transform.position.y;
			_deffaultColor = SecondPriceText.color;
		}

		private void SetPlyerInfo()
		{
			PlayerNameText.text = GameData.Instance.CurrentGameinfo.CurrentName;
			CoinsCountText.text = GameData.Instance.CurrentGameinfo.Coins.ToString();
		}

		#region Methods

		public static CharactersUiController Instance()
		{
			return _instance;
		}

		public void ButtonMenuPressed()
		{
			SceneManager.LoadMenu();
		}
		public void ButtonPressed()
		{
			ScrollMover instance = ScrollMover.Instance();

			if (instance == null || instance.ActiveCard == null)
				return;
			if (instance.ActiveCard.IsOpen)
				StartGame();
			else
				TryToBuyCharacter(instance.ActiveCard);
		}

		private void TryToBuyCharacter(CharacterCard card)
		{
			if (GameData.Instance.CurrentGameinfo.Coins >= GameData.GetCharacterInfo(card.Character).Price)
				OpenCharacter(card);
			else
				ShowMassageNotEnoughMoney();
		}

		public void ShowMassageNotEnoughMoney()
		{
			BuyMorePanel.gameObject.SetActive(true);
		}

		public void HideMassageNotEnoughMone()
		{
			BuyMorePanel.gameObject.SetActive(false);
		}

		private void OpenCharacter(CharacterCard card)
		{
			ulong price = GameData.GetCharacterInfo(card.Character).Price;
			GameData.Instance.CurrentGameinfo.Coins -= price;
			ChangePrice(GameData.Instance.CurrentGameinfo.Coins.ToString(), ((-1) * (int)price).ToString());
			card.IsOpen = true;
			GameData.UnlockCharacter(card.Character);
			InfoToCard(card);
			ScrollMover.Instance().ActivateCurrentCard();
		}

		public void StartGame()
		{
			GameData.Instance.CurrentGameinfo.CurrentCharacter = ScrollMover.Instance().ActiveCard.Character;
			SceneManager.LoadGame();
		}

		public void ChangePrice(string newPrice, string difference)
		{
			CoinsCountText.text = newPrice;
			SecondPriceText.text = difference;

			SecondPriceText.enabled = true;
			//			//Animating color
			Hashtable tweenParams = new Hashtable
			{
				{"from", _deffaultColor},
				{"to", new Color(_deffaultColor.r, _deffaultColor.g, _deffaultColor.b, 0)},
				{"time", AnimationTime/3},
				{"onupdate", "OnColorUpdated"}
			};
			ITween.ValueTo(gameObject, tweenParams);
			SecondPriceText.GetComponent<RectTransform>().position = _deffaultPosition;
			ITween.MoveTo(SecondPriceText.gameObject, ITween.Hash("y", DeffaultYPosition + AnimationShift, "time", AnimationTime, "easetype", ITween.EaseType.easeOutQuad));
		}

		public void CharacterChanged()
		{
			SecondPriceText.enabled = false;
			ITween.Stop();
		}

		private void OnColorUpdated(Color color)
		{
			SecondPriceText.color = color;
		}
		#endregion
		[Serializable]
		public class BuyingButtonContainer
		{
			#region  Variables

			public Text ButtonText;
			public Image MeatImage;
			public Text MeatText;
			public string Buytext = "Buy";
			public string GetCoinsText = "Get Coins";
			public string PlayText = "Play";

			#endregion

			#region Methods

			public void SetAsBuy()
			{
				ButtonText.text = Buytext;
				MeatText.enabled = true;
				MeatImage.enabled = true;
			}

			public void SetAsPlay()
			{
				ButtonText.text =PlayText;
				MeatText.enabled = false;
				MeatImage.enabled = false;
			}

			public void SetPriceToCard(CharacterCard card)
			{
				MeatText.text = GameData.GetCharacterInfo(card.Character).Price.ToString();
			}

			#endregion
		}

		public void InfoToCard(CharacterCard card)
		{
			if (card.IsOpen)
				CharacterIcon.sprite = GameData.GetCharacterIcon(card.Character);
			else
				CharacterIcon.sprite = UnknownCharacterSprite;
		}
	}
}