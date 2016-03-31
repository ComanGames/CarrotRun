using System;
using Assets.Script.GamePlay;
using Assets.Script.GamePlay.Data_Containers;
using Assets.Script.Menu.DataManagment;

namespace Assets.Script.DataManagment
{
	[Serializable]
	public class ScoreInfo:IComparable<ScoreInfo>
	{
		public string PlayerName;
		public ulong Score;
		public Characters Character;
		public ScoreInfo(string playerName, ulong score)
		{
			PlayerName = playerName;
			Score = score;
			Character = GameData.Instance.CurrentGameinfo.CurrentCharacter;
		}

		public int CompareTo(ScoreInfo other)
		{
			return other == null ? 1 : Score.CompareTo(other.Score);
		}
	}
}