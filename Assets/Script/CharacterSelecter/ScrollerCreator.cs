using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.DataManagment;
using Assets.Script.Menu.DataManagment;
using UnityEngine;

namespace Assets.Script.CharacterSelecter
{

	public class ScrollerCreator : MonoBehaviour
	{
		#region  Variables
		//public
		public float StartOffSet = 10;
		public float DistanceX = 1;
		public float Scale = 0.5f;
		public float PositionY = 0;
		public Transform ItemsContainer;
		public bool IsSort = true;
		public CharacterCard First;
		//private
		private Transform[] _items;
		private Vector2 _screen;
		private float _distanceX;

		private RectTransform MyRectTransform
		{
			// ReSharper disable once ConvertPropertyToExpressionBody
			get { return GetComponent<RectTransform>(); }
		}

		#endregion

		#region Methods

		public void Start()
		{
			SettingDistance();
			SetCoinsOpenCharacter();
			AddScroll();
			Destroy(this);
		}

		private void SetCoinsOpenCharacter()
		{
			if(_items==null)
				// ReSharper disable once NotResolvedInText
				throw new ArgumentNullException(@"Items are not set");
			foreach (Transform item in _items)
			{
				if (item.GetComponent<CharacterCard>())
				{
					CharacterCard card = item.GetComponent<CharacterCard>();
					card.IsOpen = !GameData.IsCharacterLocked(card.Character);
					if (card.IsOpen)
						item.GetComponent<CharacterCard>().SetSpriteOpen();
				}
			}
			CoinsCounterCharacterSelecter coinsCounter =
				transform.parent.gameObject.AddComponent<CoinsCounterCharacterSelecter>();
			coinsCounter.Start();

		}

		private void AddScroll()
		{
			var scroll = ItemsContainer.gameObject.AddComponent<ScrollMover>();
			scroll.Setter(MyRectTransform, WorldToScreen(), _distanceX,StartOffSet);
		}

		private void SettingDistance()
		{
			SetAndSortItems();
		}




		private void SetAndSortItems()
		{
			UpdateVariables();
			if (ItemsContainer != null && ItemsContainer.childCount > 0)
			{
				SetItemsPos(_items);
				SetScroll(_screen);
			}
		}

		private void UpdateVariables()
		{
			List<Transform> childerens= ItemsContainer.Cast<Transform>().ToList();
			_items = (from child in childerens
					  where child.gameObject.GetComponent<CharacterCard>() != null
					  select child).ToArray();
			_screen = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
			_distanceX = DistanceX*(_screen.x/_screen.y);
		}

		private void SetScroll(Vector2 screen)
		{
			//Set the Size
			var worldToScreen = WorldToScreen();
			var sizeX = _distanceX/worldToScreen*(_items.Length-1);
			sizeX += screen.x;
			MyRectTransform.sizeDelta = new Vector2(sizeX, 0);
		}

		private float WorldToScreen()
		{
			Vector2 worldProp = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
			var worldToScreen = worldProp.x/_screen.x;
			return worldToScreen;
		}

		private void SetItemsPos(Transform[] children)
		{
			var sizeX = _distanceX;
			var offset = ((sizeX)*(children.Length - 1))/2;
			for (var i = 0; i < children.Length; i++)
			{
				if(children[i].GetComponent<CharacterCard>())
				{
					var posX = (i*(sizeX)) - offset;
					children[i].localPosition = new Vector2(posX, PositionY);
					children[i].GetChild(0).localScale = Vector3.one*Scale;
				}
			}
		}

		#endregion
	}
}