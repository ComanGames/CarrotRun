using System;
using UnityEngine;

namespace Assets.Script.SAnimation.Bases
{
	[Serializable]
	public class CircleLinkedList
	{
		[Serializable]
		public class Node
		{
			public Node Next;
			private readonly Sprite _item;
			public Sprite GetSprite()
			{
				return _item;
			}


			public Node()
			{ }
			public Node( Node next)
			{
				Next = next;
			}

			public Node(Sprite item)
			{
				_item = item;
			}

			public Node(Sprite item, Node next)
			{
				Next = next;
				_item = item;
			}
		}

		#region Variables

		public  Action OnAnimationEnd;
		public Node FirstNode;
		private Node _current;
		public bool OneSpriteAnimation;
		public bool IsCircle = true;

		#endregion

		public CircleLinkedList()
		{

		}

		public CircleLinkedList(Sprite[] items)
		{
			if (items.Length == 1)
			{

				OneSpriteAnimation = true;
				FirstNode = new Node(items[0]);
				FirstNode.GetSprite();
				_current = FirstNode;
			}
			Node last = new Node(items[items.Length - 1]);
			Node tempLast = last;
			for (int i = items.Length - 2; i >= 0; i--)
			{
				Node temp = new Node(items[i], tempLast);
				tempLast = temp;
			}
			FirstNode = tempLast;

		}

		public Sprite Next()
		{
			if (OneSpriteAnimation)
				return FirstNode.GetSprite();
			if (_current == null)
				_current = FirstNode;

			if (_current.Next == null)
			{
				if (OnAnimationEnd != null)
				{
					OnAnimationEnd();
				}
				if (IsCircle)
				{
					_current = FirstNode;
					if (_current == FirstNode)
						return _current.GetSprite();
				}
				else
					return _current.GetSprite();
			}
			_current = _current.Next;
			return _current.GetSprite();
		}

		public void Reset()
		{
			_current = FirstNode;
		}

	}
}