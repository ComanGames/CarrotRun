using UnityEngine;

namespace Assets.Script.iTween
{
	public class TweenUiMoving : MonoBehaviour
	{
		public Vector3 MoveOn;
		public float AnimTime;
		private Vector3 _myPosition;
		private RectTransform _uGuiElement;

		public void Awake()
		{
			_uGuiElement = GetComponent<RectTransform>();
			_myPosition = _uGuiElement.anchoredPosition;
		}

		public void OnEnable()
		{
			SlideIn();
		}
		public void SlideIn()
		{
			_uGuiElement.anchoredPosition = _myPosition;
			Vector2 targetPos = _myPosition + MoveOn;
			ITween.ValueTo(_uGuiElement.gameObject, ITween.Hash(
				"from", _uGuiElement.anchoredPosition,
				"to", targetPos,
				"time", AnimTime,
				"onupdatetarget", gameObject,
				"easetype", ITween.EaseType.easeOutCirc,
				"onupdate", "MoveGuiElement"));
		}

	public void MoveGuiElement(Vector2 position)
	{
		_uGuiElement.anchoredPosition = position;
	}
}
}