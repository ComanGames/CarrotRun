using System;
using System.Collections;
using Assets.Script.iTween;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay
{
	[RequireComponent(typeof(Text))]
	public class NumberAnimaiton : MonoBehaviour
	{
		#region Variables
		public  Action AnimationDone;
		private Text _text;
		#endregion
		public void Start()
		{
			_text = GetComponent<Text>();
		}

		public void RunAnimation(float time,int from, int to)
		{
			Hashtable ht = new Hashtable
			{
				{"time", time},
				{"from", (float) from},
				{"to", (float) to},
				{"easetype", ITween.EaseType.easeOutQuad},
				{"onupdate", "UpdateValue"},
				{"oncompleteparams", "OnComplete"}
			};
			ITween.ValueTo(gameObject,ht);
		}

		public void OnComplete()
		{
			AnimationDone?.Invoke();	
		}

		public void UpdateValue(float value)
		{
			_text.text = ((int) value).ToString();
		}
	}
}