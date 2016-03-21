using System;
using Assets.Script.SAnimation.Bases;
using UnityEngine;

namespace Assets.Script.SAnimation.Bakers
{
	[Serializable]
	public class CLLContainer : ScriptableObject
	{
		#region Variables

		public Sprite[] Sprites;
		#endregion

		public CircleLinkedList GetCircleLinkedList()
		{
			return new CircleLinkedList(Sprites);
		}

	}
}