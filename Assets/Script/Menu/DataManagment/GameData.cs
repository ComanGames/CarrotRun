using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.DataManagment;
using Assets.Script.GamePlay;
using Assets.Script.GamePlay.Data_Containers;
using UnityEngine;
using CharacterInfo = Assets.Script.DataManagment.CharacterInfo;

namespace Assets.Script.Menu.DataManagment
{
	[Serializable]
	public class GameData
	{
		protected GameData()
		{
			AddAllCharactersInfo();
			if(!FileManager.IsLastRecordExists())
				SetCharactersByDeffault();
		}

		private void AddAllCharactersInfo()
		{
			CharactersInfo = new Dictionary<Characters, CharacterInfo>();
			CharactersAsset charactersAsset = Resources.Load<CharactersAsset>("Characters");
			foreach (var characterInfo in charactersAsset.CharactersInfo)
				CharactersInfo.Add(characterInfo.Charcter,characterInfo);
		}

		private void SetCharactersByDeffault()
		{
			CurrentGameinfo.OpenCharacters = new Dictionary<Characters, bool>();
			//Set all characters open or closed 
			foreach (var characterInfo in CharactersInfo)
				CurrentGameinfo.OpenCharacters.Add(characterInfo.Key,!characterInfo.Value.IsLocked);
		}

		public static bool IsInstance()
		{
			return RealInstance != null;
		}
		public static void UnlockCharacter(Characters character)
		{
			Instance.CurrentGameinfo.OpenCharacters[character] = true;
			FileManager.SaveGameRecord();
		}
		public static void AddNewScore(ScoreInfo  scoreInfo) 
		{
			if(scoreInfo==null)
				throw new ArgumentNullException();
			Instance.LastScoreInfo = scoreInfo;
			FileManager.SaveGameRecord();
			if (scoreInfo.Score>Instance.HighScore)
			{
				Instance.HighScore = scoreInfo.Score;
			}
			if(Instance.PlayerRecords.Count<MaxLengthOfScore)
			{
				Instance.PlayerRecords.Add(scoreInfo);
				SortPlayerRecords();
				FileManager.SaveScore();
			}
			else 
			{

				SortPlayerRecords();
				if(Instance.PlayerRecords[Instance.PlayerRecords.Count-1].Score<scoreInfo.Score)
				{
					RemoveLastRecrod();
					Instance.PlayerRecords.Add(scoreInfo);
					FileManager.SaveScore();
				}
			}
		}

		public static void SortPlayerRecords()
		{
				Instance.PlayerRecords.Sort();
				Instance.PlayerRecords.Reverse();
		}

		private static void RemoveLastRecrod()
		{
			do
			{
				Instance.PlayerRecords.RemoveAt(Instance.PlayerRecords.Count - 1);
			} while (Instance.PlayerRecords.Count >= MaxLengthOfScore - 1);
		}

		public static Sprite GetCharacterIcon(Characters character)
		{
			CharacterInfo characterInfo;
			Instance.CharactersInfo.TryGetValue(character, out characterInfo);
			if(characterInfo==null)
				throw new NullReferenceException("Such character do not exist "+character.ToString());
			if(characterInfo.Image==null)
				throw new NullReferenceException("We don't have image for"+character.ToString());
			return characterInfo.Image;

		}
		public static GameData Instance
		{
			get
			{
				// ReSharper disable once ConvertIfStatementToNullCoalescingExpression
				if(RealInstance==null)
					RealInstance = new GameData();
				return RealInstance;
			}
			set
			{
				if(RealInstance!=null)
					return;
				RealInstance = value;

			}
		}
		private const uint MaxLengthOfScore = 120;
		protected static GameData RealInstance;
		public ulong HighScore;
		public List<ScoreInfo> PlayerRecords
		{
			get
			{
				// ReSharper disable once ConvertIfStatementToNullCoalescingExpression
				if (_playerRecords == null)
				{
					_playerRecords = FileManager.LoadScore();
				}
				return _playerRecords;
			}
		}
		private List<ScoreInfo> _playerRecords;
		public Dictionary<Characters, CharacterInfo> CharactersInfo ;
		public Gameinfo CurrentGameinfo
		{
			get
			{
				// ReSharper disable once ConvertIfStatementToNullCoalescingExpression
				if (_currentGameInfo == null)
				{
					_currentGameInfo = FileManager.LoadGameInfo();
				}
				return _currentGameInfo;
			}
		}

		public SoundSettings GameSoundSettings
		{
			get { return CurrentGameinfo.GameSoundSettings; }
			set { CurrentGameinfo.GameSoundSettings = value; }
		}
		private Gameinfo _currentGameInfo;
		public ScoreInfo LastScoreInfo;
		public bool IsFirstGame
		{
			// ReSharper disable once ConvertPropertyToExpressionBody
			get { return !FileManager.IsLastRecordExists(); }
		}
		public static bool IsCharacterLocked(Characters character)
		{
			return !Instance.CurrentGameinfo.OpenCharacters[character];
		}
		public static CharacterInfo GetCharacterInfo(Characters character)
		{
			CharacterInfo result;
			Instance.CharactersInfo.TryGetValue(character, out result);
			return result;
		}

		public static ScoreInfo GetTopGameInfo()
		{
			if (Instance.PlayerRecords.Count < 1)
				return new ScoreInfo("",0);
			return Instance.PlayerRecords.Max();
		}
	}
}