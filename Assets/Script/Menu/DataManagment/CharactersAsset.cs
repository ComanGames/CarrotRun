using System;
using Assets.Script.GamePlay;
using Assets.Script.GamePlay.Data_Containers;
using UnityEngine;

namespace Assets.Script.DataManagment
{
	[Serializable]
	public class CharacterInfo
	{
		public Characters Charcter;
		public bool IsLocked;
		public Sprite Image;
		public ulong Price;
		public int SceneNumber;
	}
	[Serializable]
	public class CharactersAsset : ScriptableObject
	{
		#region Variables
		public CharacterInfo[] CharactersInfo;
		#endregion
	}
}