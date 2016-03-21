

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.DataManagment
{
	public class ResourceUnloder : MonoBehaviour
	{
		public List<Object> ToUnlodad;
		public static ResourceUnloder Instance;
		public void Awake()
		{
			Instance = this;
		}

		public static void UnoadResources()
		{
			if (Instance != null&&Instance.ToUnlodad!=null)
			{
				for (int i = 0; i < Instance.ToUnlodad.Count; i++)
				{
					Resources.UnloadAsset(Instance.ToUnlodad[i]);
					Instance.ToUnlodad[i] = null;
				}
				Instance.ToUnlodad = null;
			}
		}
	}
}