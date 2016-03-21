#if UNITY_EDITOR
using UnityEngine;

namespace Assets.Script.SAnimation.Bakers
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class AnimationBaker : MonoBehaviour
	{
		public bool DestorIteself;
		public bool Bake;
		public Sprite[] Sprites;

		public void OnDrawGizmos()
		{
			if (Bake)
			{
				if (Sprites != null && Sprites.Length > 0 )
				{
					var sa = gameObject.AddComponent<SAnimation>();
					sa.Container = SerializationUtilits.SerialazingAnimation(Sprites);
					gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[0];
					Bake = false;
					if (DestorIteself)
						DestroyImmediate(this);
				}
				else
					Bake = false;
			}
		}
	}
}
#endif