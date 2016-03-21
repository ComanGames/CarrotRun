using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.CharacterSelecter
{
	public class ScrollMover : MonoBehaviour
	{
		#region  Variables

		public RectTransform  ScrollerUi;
		public float Proportion;
		public float PosY;
		public float RangeX;
		private float _activeCardX;
		private Stack<CharacterCard> _leftCards;
		private Stack<CharacterCard> _rightCards;
		public CharacterCard ActiveCard;
		public Color PlayColor = Color.blue;
		public Color SellColor = Color.yellow;
		#endregion

		#region Methods
		private static ScrollMover _instance;
		public static ScrollMover Instance()
		{
			return _instance;
		}

		public void Update()
		{
			transform.position = new Vector3(ScrollerUi.localPosition.x*Proportion, PosY);
			UpdateActiveCard();
		}

		public void Setter(RectTransform scrollerUi, float proportion, float distanceX,float startOffSet)
		{
			_instance = this;
			BuyingContainer = CharactersUiController.Instance().BuyingContainer;
			ScrollerUi = scrollerUi;
			Proportion = proportion;
			RangeX = distanceX/2;

			_leftCards = new Stack<CharacterCard>();
			_rightCards = new Stack<CharacterCard>();
			var temp = (from t in transform.Cast<Transform>()
				where t.gameObject.GetComponent<CharacterCard>() != null
				select t).ToArray();

			if (temp.Length < 1)
				throw new InvalidOperationException("We got no elements in for moving");
			foreach (var t in temp)
			{
				var card = t.gameObject.GetComponent<CharacterCard>();
				if (t.position.x < -RangeX)
					_leftCards.Push(card);
				else if (t.position.x > RangeX)
					_rightCards.Push(card);
				else
					SetActiveCard(card);
			}
			_rightCards = new Stack<CharacterCard>(_rightCards.ToArray());
			//Making position just prefect on the First item
			scrollerUi.localPosition = new Vector3((scrollerUi.sizeDelta.x/2), 0,0);
		}


		private void SetActiveCard(CharacterCard card)
		{
			ActiveCard = card;
			_activeCardX = card.transform.localPosition.x;
			card.MakeBig();
			CharactersUiController.Instance().InfoToCard(card);
			if(BuyingContainer==null)
				return;
			if (!card.IsOpen)
			{
				BuyingContainer.SetPriceToCard(card);
				BuyingContainer.SetAsBuy();
			}
			else
			{
				BuyingContainer.SetAsPlay();
			}
		}

		public CharactersUiController.BuyingButtonContainer BuyingContainer { get; set; }

		private CharacterCard RemvoeActiveCard()
		{
			CharactersUiController.Instance().CharacterChanged();
			ActiveCard.transform.parent = transform;
			ActiveCard.transform.localPosition = new Vector3(_activeCardX, ActiveCard.transform.position.y);
			var tempCard = ActiveCard;
			ActiveCard.MakeNormal();
			ActiveCard = null;
			return tempCard;
		}

		public void UpdateActiveCard()
		{
			float RangeX2 = RangeX/2;
			//We got 3 situation
			//When Active Card equals null
			if (ActiveCard == null)
			{
				if (_leftCards.Count > 0 && _leftCards.Peek().transform.position.x >= -RangeX2)
					SetActiveCard(_leftCards.Pop());
				else if (_rightCards.Count > 0 && _rightCards.Peek().transform.position.x <=RangeX2)
					SetActiveCard(_rightCards.Pop());
				//When Active Card notequal null
			}
			else
			{
				if (_rightCards.Count > 0 && _rightCards.Peek().transform.position.x < RangeX)
				{
					_leftCards.Push(RemvoeActiveCard());
					SetActiveCard(_rightCards.Pop());
				}
				//Else when not one of over cards is not out but maybe we should remove over Active Card every
				else if (_leftCards.Count > 0 && _leftCards.Peek().transform.position.x > -RangeX)
				{
					_rightCards.Push(RemvoeActiveCard());
					SetActiveCard(_leftCards.Pop());
				}
				//it is possible that one of th them is empty but over Active stell not equals null so we need to make it null
				else
				{
					if (_leftCards.Count == 0 && (transform.position.x + _activeCardX > RangeX2))
						_rightCards.Push(RemvoeActiveCard());
					else if (_rightCards.Count == 0 && (transform.position.x + _activeCardX < -RangeX2))
						_leftCards.Push(RemvoeActiveCard());
				}
			}
		}

		#endregion

		public void ActivateCurrentCard()
		{
			BuyingContainer.SetAsPlay();
			ActiveCard.OpenCard();
		}
	}
}