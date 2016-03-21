using UnityEngine;

namespace Assets.Script.GamePlay
{
	public class Coin : MonoBehaviour
	{

		public void OnTriggerStay2D(Collider2D collision)
		{
			SayAboutCoin(collision);
		}

		private void SayAboutCoin(Collider2D collision)
		{
			if (collision.tag == "Player")
				GameManager.Instance.AddCoin(gameObject);
		}

	}
}