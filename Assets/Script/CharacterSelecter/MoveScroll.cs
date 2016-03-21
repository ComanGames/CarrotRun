

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Script.CharacterSelecter
{
	public class MoveScroll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		public RectTransform ScrollPoll;
		public int Count;
		private bool isHolding;

		private void MoveScroller()
		{
			float distance = ScrollPoll.sizeDelta.x/Count;
			ScrollPoll.Translate(distance, 0, 0);
		}

		public void Update()
		{
			if(isHolding)
				MoveScroller();
		}



		public void OnPointerExit(PointerEventData eventData)
		{
			isHolding = false;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			isHolding = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			isHolding = false;
		}
	}
}