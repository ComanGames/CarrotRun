using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay.VisualEffects
{
	public class SkillBg : MonoBehaviour
	{
		public static SkillBg Instance;
		public  static Image BgImage => Instance._myImage;
		private Image _myImage;
		public void Create()
		{
			Instance = this;
			_myImage = GetComponent<Image>();
		}

	}
}