using UnityEngine;

namespace Assets.Script.GamePlay
{
	public class Problem : MonoBehaviour
	{

		public void OnTriggerStay2D(Collider2D collision)
		{
			if (!GameManager.Instance.Over && collision.tag == "Player" && !GameManager.Instance.Over)
			{
				GameManager.Instance.GameOver();
			}
		}
	}
}