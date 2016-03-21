using Assets.Script.Menu.DataManagment;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Script.DataManagment
{


	public class CoinsCounterCharacterSelecter : MonoBehaviour
	{
		#region Variables
		public Text CoinText;
		#endregion
		public static  CoinsCounterCharacterSelecter Instance;
		public void Start()
		{
			if(Instance!=null)
				Destroy(Instance);
			Instance = this;
			if (CoinText != null)
				CoinText.text = GameData.Instance.CurrentGameinfo.Coins.ToString();
			else if (CoinText != null) CoinText.text = "0";

		}
	}
}